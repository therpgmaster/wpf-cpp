#pragma once
#include <string>
#include <sstream>
#include <vector>

class FileRW
{
	bool opSuccess = false;
	std::stringstream buf{}; // file buffer
public:
	// copies file to buffer (object implicitly converts to stringstream&)
	FileRW(const std::string& fp);
	// appends data to file (object implicitly converts to bool for error checking)
	FileRW(const std::string& fp, const std::string& inData);

	operator std::stringstream&() { return buf; }
	operator bool() const { return opSuccess; }
};


class CSV
{
	// parsed data (lines, fields)
	std::vector<std::vector<std::string>> csv;

public:
	// open file and parse CSV data into buffer
	bool load(const std::string& filePath, const std::string& delimiter = ",");
};
