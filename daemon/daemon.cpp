#include <iostream>
#include "ipc.h"
#include "debug.h"

int main()
{
    debug();

    ipc ipc{};
    for (;;)
    {
        std::string str;
        if (ipc.receive(str)) std::cout << "received: " << str << "\r\n";
    }
}

