#include "THREADPOOL.h"
#include <windows.h>
#include <cstdlib>
#include <stdlib.h>
#include <iostream>
#include <fstream>
#include <stdexcept>
#include <conio.h>

using namespace std;

static DWORD WINAPI ThreadProc(LPVOID lpParam);

THREADPOOL *ThreadPool;
HANDLE hFileMutex;
HANDLE hNumberMutex;
ofstream fDest;

int main(int argc, char *argv[])
{

    hFileMutex = CreateMutex(NULL, false, NULL);
    QueueTasks *queuetask = new QueueTasks();

    for (int i = 0; i < 5; i++)
    {
        int temp = rand() % 31 - 20;
        cout << temp << endl;
        queuetask->AddTask(new Task(temp));
    }
    
    fDest.open("file.txt");
    
    ThreadPool = new THREADPOOL(4, (void *) &ThreadProc);
    ThreadPool->SetQueueTasks(queuetask);
    ThreadPool->ProcessTask();
    bool flag = true;
    char key;
    
    while (flag)
    {
        cout << "1 : Process tasks" << endl;
        cout << "2 : Add task" << endl;
        cout << "3 : Exit" << endl;
        key = getch();
        
        switch (key)
        {
            case '1':
               break;
            case '2':
               break;
            case '3':
               flag = false;
               break;
        };
        while (ThreadPool->GetNumber() < 0)
              ThreadPool->AddThread((void *) &ThreadProc);
        
    }
    fDest.close();
    system("PAUSE");
    return EXIT_SUCCESS;
}

static DWORD WINAPI ThreadProc(LPVOID lpParam)
{
   DWORD ThreadId = GetCurrentThreadId();
   WaitForSingleObject(hFileMutex, INFINITE);
   fDest << "Thread " << ThreadId << " logged in succesfully" << endl;
   fDest.flush();
   ReleaseMutex(hFileMutex);
   //to do while here
   WaitForSingleObject(ThreadPool->QueueTask->hQueueMutex, INFINITE);
   Task *temp = ThreadPool->QueueTask->GetAndDeleteTask();
   int sleep = temp->GetTask();
   ReleaseMutex(ThreadPool->QueueTask->hQueueMutex);
   
   WaitForSingleObject(hFileMutex, INFINITE);
   fDest << "Thread " << GetCurrentThreadId() << " started task" << endl;
   ReleaseMutex(hFileMutex);
   
   try
   {
                            
       if (sleep < 0)
       {
          throw std::logic_error("Incorrect task");
       }
       else
          Sleep(sleep);
   }
   catch (std::logic_error e)
   {
       WaitForSingleObject(hFileMutex, INFINITE);
       fDest << "Thread " << GetCurrentThreadId() << " throw exeption and exited" << endl;
       ReleaseMutex(hFileMutex);
        
       ThreadPool->RemoveThread();
        
       ExitThread(1);
   }     
   
   WaitForSingleObject(hFileMutex, INFINITE);
   fDest << "Thread " << GetCurrentThreadId() << " finished task" << endl;
   ReleaseMutex(hFileMutex);
   return 0;
}
