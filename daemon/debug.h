#pragma once

#include <iostream>
#include "parser.h"

void debug() 
{
	std::cout << "enter file path >";
	std::string filename;
	std::cin >> filename;
	std::cout << "\nloading csv " << filename;

	CSV csv{};
	for(;;) 
	{
		if (!csv.load(filename)) { std::cout << "\n could not load file"; }

		// print data line by line
		for (const auto& record : csv.get())
		{
			std::cout << "\n";
			for (const auto& v : record)
			{
				std::cout << v << "-";
			}
		}
		std::cout << "\n";

		// test append to file
		std::cout << "\nenter new record >";
		std::string nrec;
		std::cin >> nrec;
		if (!csv.addRecord_string(nrec)) { std::cout << "\n could not write to file"; }
	}
	
}