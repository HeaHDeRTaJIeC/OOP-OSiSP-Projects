// OSISP3.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <conio.h>
#include <iostream>

#define SHARED_MEMORY_NAME L"SharedMemory"
#define SHARED_MEMORY_SIZE 100

using namespace std;

int _tmain(int argc, _TCHAR* argv[])
{
	bool flag;
	LPCSTR lpBuf;
	HANDLE hMutex = CreateMutex(NULL, FALSE, L"Global\\ProcessMutex");
	if (GetLastError() == ERROR_ALREADY_EXISTS)
		cout << "Error, mutex created" << endl;
	CHAR Buffer[30] = "Hello shared memory!";
	CHAR EmptyBuffer[30];
	HANDLE hMap = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, SHARED_MEMORY_SIZE, SHARED_MEMORY_NAME);
	if (flag = (GetLastError() == ERROR_ALREADY_EXISTS))
		hMap = OpenFileMapping(FILE_WRITE_ACCESS, FALSE, SHARED_MEMORY_NAME);
	if (hMap == NULL)
	{
		cout << "hMap = null" << endl;
		_getch();
		return false;
	}
 
	lpBuf = (LPSTR) MapViewOfFile(hMap, FILE_WRITE_ACCESS, 0, 0, SHARED_MEMORY_SIZE);
	if (lpBuf == NULL)
	{
		cout << "lpBuf = null, " << endl;
		_getch();
		return false;
	}
	if (flag == false)
	{
		cout << Buffer << endl;
		CopyMemory((PVOID)lpBuf, Buffer, 30);
	}

	_getch();

	//Critical section
	for (int i = 0; i < 4; i++)
	{
		cout << "Wait for critical section" << endl;
		WaitForSingleObject(hMutex, INFINITE);
		cout << "Enter critical section" << endl;
		strncpy(EmptyBuffer, lpBuf, 30);
		cout << EmptyBuffer << endl;
		Sleep(5000);
		ReleaseMutex(hMutex);
		cout << "Leave critical section" << endl;
	}

	cout << "Program end" << endl;

	UnmapViewOfFile(lpBuf);
	CloseHandle(hMap);
	CloseHandle(hMutex);

	system("PAUSE");
	return 0;
}

