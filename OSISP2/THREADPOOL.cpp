#include "stdafx.h"
#include "THREADPOOL.h"



Task::Task()
{
	Milisec = 1;
}

Task::Task(long K)
{
	Milisec = K;
}

void Task::SetTask(long K)
{
	Milisec = K;
}

long Task::GetTask()
{
	return Milisec;
}

QueueTasks::QueueTasks()
{
	Count = 0;
	QueueTask = new std::queue<Task *>();
	hQueueMutex = CreateMutex(NULL, false, NULL);
}

QueueTasks::~QueueTasks()
{ 
	CloseHandle(hQueueMutex);
	while (QueueTask->front() != NULL)
		QueueTask->pop();
}

void QueueTasks::AddTask(Task *task)
{
	WaitForSingleObject(hQueueMutex, INFINITE);
	++Count;
	QueueTask->push(task);
	ReleaseMutex(hQueueMutex);
}

Task * QueueTasks::GetAndDeleteTask()
{
	Task *temp;
	WaitForSingleObject(hQueueMutex, INFINITE);
	if (Count <= 0)
	{
		ReleaseMutex(hQueueMutex);
		return NULL;
	}
	else
	{
		--Count;
		temp = QueueTask->front();
			QueueTask->pop();
		ReleaseMutex(hQueueMutex);
		return temp;
	}
}

bool QueueTasks::IsEmpty()
{
	bool temp;
	WaitForSingleObject(hQueueMutex, INFINITE);
	Count == 0 ? temp = true : temp = false;
	ReleaseMutex(hQueueMutex);
	return temp;
}

THREADPOOL::THREADPOOL(int N, void *ptrFunc)
{
	HANDLE temp;
	hThreads = new HANDLE[N];
	hThreadMutex = CreateMutex(NULL, false, NULL);
    hNumberMutex = CreateMutex(NULL, false, NULL);
    Number = 0; 
	for (int i = 0; i < N; i++)
	{
		temp = CreateThread(NULL, 100, (LPTHREAD_START_ROUTINE) ptrFunc, NULL, CREATE_SUSPENDED, NULL);
		if (temp != NULL)
	    {
			hThreads[i] = temp;
			++Number;
        }
	}
}

THREADPOOL::~THREADPOOL(void)
{
	delete [] hThreads;
}

void THREADPOOL::SetQueueTasks(QueueTasks *queueTask)
{
	QueueTask = queueTask;
}

void THREADPOOL::ProcessTask()
{
	WaitForSingleObject(hNumberMutex, INFINITE);
	int temp = Number;
	ReleaseMutex(hNumberMutex);
	for (int i = 0; i < temp; i++)
		ResumeThread(hThreads[i]);

}

void THREADPOOL::AddThread(void * ptrFunc)
{
     WaitForSingleObject(hNumberMutex, INFINITE);
     HANDLE temp = CreateThread(NULL, 100, (LPTHREAD_START_ROUTINE) ptrFunc, NULL, 0, NULL);  
     ++Number;   
     ReleaseMutex(hNumberMutex);
}

void THREADPOOL::RemoveThread()
{
     WaitForSingleObject(hNumberMutex, INFINITE);
     --Number;
     ReleaseMutex(hNumberMutex);
}

int THREADPOOL::GetNumber()
{
    WaitForSingleObject(hNumberMutex, INFINITE);
    int temp = Number;
    ReleaseMutex(hNumberMutex);
    return temp;
}
