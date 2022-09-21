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
	opSuccess = true;
}

FileRW::FileRW(const std::string& fp, const std::string& inData)
{
	std::ofstream s(fp.c_str(), std::ios_base::out | std::ios_base::app); // open output stream, append mode
	if (s.fail()) { return; }
	s << inData; // write to file
	s.close();
	opSuccess = true;
}

bool CSV::load(const std::string& filePath, const std::string& delimiter)
{
	std::stringstream& buf = FileRW(filePath); // copy to temporary buffer
	csv.clear();
	// parse lines from file
	for (std::string line; std::getline(buf, line);) 
	{
		std::vector<std::string> lineFields;
		std::string field;
		// parse characters from line
		for (const char& c : line)
		{
			if (std::string(1,c) != delimiter) { field.push_back(c); } // add character to field
			else 
			{
				// end of field hit, reset and move on to the next one
				lineFields.push_back(field);
				field.clear();
			}
		}
		csv.push_back(lineFields);
	}
}