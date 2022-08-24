#include <iostream>
#include "ipc.h"

int main()
{
    ipc ipc{};
    for (;;)
    {
        std::string str = ipc.receive();
        if (!str.empty()) std::cout << "received: " << str << "\r\n";
    }
}

