#include <iostream>
#include "ipc.h"

int main()
{
    ipc ipc{};
    for (;;)
    {
        std::string str;
        if (ipc.receive(str)) std::cout << "received: " << str << "\r\n";
    }
}

