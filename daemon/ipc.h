#pragma once

#include <windows.h>
#include <iostream>
#include <string.h>

class ipc 
{
    HANDLE fileHandle;
    
    void readString(char* output) 
    {
        ULONG read = 0;
        int index = 0;
        do { ReadFile(fileHandle, output + index++, 1, &read, NULL); } 
        while (read > 0 && *(output + index - 1) != 0);
    }
public:
    ipc() 
    {
        fileHandle = CreateFileW(TEXT("\\\\.\\pipe\\rpgpipe"), GENERIC_READ | GENERIC_WRITE, FILE_SHARE_WRITE, NULL, OPEN_EXISTING, 0, NULL);
    }
    bool receive(std::string& dataOut) /* may block */
    {
        char* buffer = new char[100];
        memset(buffer, 0, 100);
        readString(buffer);
        std::string str = std:string(buffer);
        delete[] buffer;
        return str;
    }

    // send data to server - TODO
    //const char* msg = "hello from c++\r\n";
    //WriteFile(fileHandle, msg, strlen(msg), nullptr, NULL);

};
