#pragma once
#include <string>
#include <sstream>
#include <vector>

class FileRW
{
	bool opOK = false; // indicates whether the r/w operation succeeded
	std::stringstream buf{}; // file buffer
public:
	// copies file to buffer (object implicitly converts to stringstream& or bool)
	FileRW(const std::string& fp);
	// appends data to file (object implicitly converts to bool for error checking)
	FileRW(const std::string& fp, const std::string& inData);

	operator std::stringstream&() { return buf; }
	operator bool() const { return opOK; }
};


class CSV
{
	// loaded, parsed data (lines, fields)
	std::vector<std::vector<std::string>> csv{};
	std::string delim; // usually comma
	std::string filePathPrev{}; // file path at last call to load() function
	bool onlyWhitespace(const std::string& s) const;
public:
	CSV(const std::string& delimiter = ",");
	// reads file and parse CSV data into buffer
	bool load(const std::string& filePath);

	// returns a value from the CSV data
	const std::string& get(uint32_t recordIndex, uint32_t fieldIndex) const;
	// for direct array-style read, although get(i,j) is safer for random access
	const std::vector<std::vector<std::string>>& get() const { return csv; }
	// assumes labels are on the first line
	const std::vector<std::string>& getLabels() const { return csv[0]; }

	// writes record to file, targets the last loaded file if none specified, automatically reloads the file
	bool addRecord(const std::vector<std::string>& rec, const std::string& filePath = std::string());
	bool addRecord_string(const std::string& recstr); // for debugging only
};
