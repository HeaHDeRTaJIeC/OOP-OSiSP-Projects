// OSISP2.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "THREADPOOL.h"
#include <windows.h>
#include <cstdlib>
#include <stdlib.h>
#include <iostream>
#include <fstream>
#include <stdexcept>
#include <conio.h>
#include <time.h>

using namespace std;

static DWORD WINAPI ThreadProc(LPVOID lpParam);

THREADPOOL *ThreadPool;
HANDLE hFileMutex;
ofstream fDest;

int _tmain(int argc, _TCHAR* argv[])
{
	int MaxNumberThreads = 4;
    int NumberThreads = 4;
    hFileMutex = CreateMutex(NULL, false, NULL);
    QueueTasks *queuetask = new QueueTasks();
    srand(time(NULL));

    for (int i = 0; i < 100; i++)
    {
        int temp = rand() % 151 - 5;
        cout << temp << endl;
        queuetask->AddTask(new Task(temp));
    }
    
    fDest.open("file.txt");
    
    ThreadPool = new THREADPOOL(NumberThreads, (void *) &ThreadProc);
    ThreadPool->SetQueueTasks(queuetask);
    bool flag = true;
    bool ControlFlag = false;
    bool MaxReached = false;
    char key;
    
    while (flag)
    {
        cout << "1 : Process tasks" << endl;
        cout << "2 : Add task" << endl;
        cout << "3 : Exit" << endl;
        key = _getch();
        switch (key)
        {
            case '1':
               ThreadPool->ProcessTask();
               ControlFlag = true;
               break;
            case '2':
               break;
            case '3':
               flag = false;
               break;
        };
        Sleep(1000);

        
    }
    fDest.close();
    system("PAUSE");
    return EXIT_SUCCESS;
}

static DWORD WINAPI ThreadProc(LPVOID lpParam)
{  
   DWORD ThreadId = GetCurrentThreadId();                          //log
   Task *temp;                                                   
   int sleep;
   WaitForSingleObject(hFileMutex, INFINITE);
   fDest << "Thread " << ThreadId << " logged in successfully" << endl;
   fDest.flush();
   ReleaseMutex(hFileMutex);
   
   while (!(ThreadPool->QueueTask->IsEmpty()))   //wait for task
   {
       temp = ThreadPool->QueueTask->GetAndDeleteTask();                                        //get task
       if (temp != NULL)                                                                     
          sleep = temp->GetTask();
       else
       {
           WaitForSingleObject(hFileMutex, INFINITE);
           fDest << "Incorrect task" << endl;
           fDest << "Thread " << ThreadId << " throw exception" << endl;
           fDest.flush();
           ReleaseMutex(hFileMutex);
       }
       
       
       WaitForSingleObject(hFileMutex, INFINITE);                                                        //log start
       fDest << "Thread " << ThreadId << " started task " << sleep << endl;
       fDest.flush();
       ReleaseMutex(hFileMutex);
       
       try                                                                                               //try task 
       {                    
           sleep <= 0 ? throw 1 : Sleep(sleep);
       }
       catch (int a)                                                                                     //catch error
       {
           WaitForSingleObject(hFileMutex, INFINITE);
           fDest << "Thread " << ThreadId << " throw exception" << endl;
           fDest.flush();
           ReleaseMutex(hFileMutex);
       }     

       WaitForSingleObject(hFileMutex, INFINITE);                                                        //log finish task
       fDest << "Thread " << ThreadId << " finished task " << sleep << endl;
       fDest.flush();
       ReleaseMutex(hFileMutex);
   }
   return 0;
}