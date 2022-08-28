#pragma once
#include <string>
#include <vector>

class EventHandler 
{
	class Event 
	{
		std::string trigger{};
	public:
		Event(std::string trigger_) : trigger{ trigger_ } {};

		bool operator==(const std::string& s) { return trigger == s; }
		bool operator!=(const std::string& s) { return trigger != s; }

	};
	
};
