#include "parser.h"

#include <iostream>
#include <fstream>
#include <filesystem>

FileRW::FileRW(const std::string& fp) 
{
	std::ifstream s(fp.c_str()); // open input stream
	if (s.fail()) { return; }
	buf << s.rdbuf(); // read whole file to buffer
	s.close();
	opOK = true;
}

FileRW::FileRW(const std::string& fp, const std::string& inData)
{
	std::ofstream s(fp.c_str(), std::ios_base::out | std::ios_base::app); // open output stream, append mode
	if (s.fail()) { return; }
	s << inData; // write to file
	s.close();
	opOK = true;
}

CSV::CSV(const std::string& delimiter) : delim{ delimiter } {};

bool CSV::load(const std::string& filePath)
{
	auto file = FileRW(filePath); // copy to temporary buffer
	std::stringstream& buf = file; // implicitly get buffer
	if (!file) { return false; }
	filePathPrev = filePath;
	csv.clear();
	// parse lines (records) from file
	for (std::string line; std::getline(buf, line);) 
	{
		std::vector<std::string> lineFields; // record
		std::string field;
		// parse characters from line
		for (const char& c : line)
		{
			if (std::string(1, c) != delim) { field.push_back(c); } // add character to field
			else
			{
				// end of field hit (delimiter), add, reset and move on to the next one
				if (!field.empty() && !onlyWhitespace(field)) { lineFields.push_back(field); }
				field.clear();
			}
		}
		// end of line hit, add last field (removes the need for trailing delimiter)
		if (!field.empty() && !onlyWhitespace(field)) { lineFields.push_back(field); }
		if (!lineFields.empty()) { csv.push_back(lineFields); } // add record
	}
	return true;
}

bool CSV::onlyWhitespace(const std::string& s) const
{
	for (const char& c : s) { if (!isspace(c)) { return false; } }
	return true;
}

const std::string& CSV::get(uint32_t recordIndex, uint32_t fieldIndex) const
{
	if (recordIndex >= csv.size() || fieldIndex >= csv[recordIndex].size()) { return std::string(); }
	return csv[recordIndex][fieldIndex];
}

bool CSV::addRecord(const std::vector<std::string>& rec, const std::string& filePath)
{
	if (rec.empty()) { return false; }
	auto fp = (filePath.empty()) ? filePathPrev : filePath; // use last known file path if none specified
	
	std::string recstr;
	recstr += "\n"; // begin record on a new line (CSV convention)
	for (const auto& f : rec)
	{
		recstr += f; // combine field(s) into record string
		if (&f != &rec.back()) { recstr += delim; } // add delimiter unless this is the last field
	}

	bool w = FileRW(fp, recstr); // write to file
	if (w) { load(fp); } // reload the file to ensure synchronization
	return w;
}

bool CSV::addRecord_string(const std::string& recstr) 
{
	std::vector<std::string> rec;
	std::string f;
	for (const auto& c : recstr) 
	{
		if (std::string(1, c) != delim) { f += c; }
		else { rec.push_back(f); f.clear(); }
	}
	if (!f.empty()) { rec.push_back(f); }
	return addRecord(rec);
}