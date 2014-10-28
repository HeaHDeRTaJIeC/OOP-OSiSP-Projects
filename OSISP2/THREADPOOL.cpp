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
	std::queue<Task *> *QueueTask = new std::queue<Task *>();
	hQueueMutex = CreateMutex(NULL, FALSE, NULL);
}

QueueTasks::~QueueTasks()
{ 
	CloseHandle(hQueueMutex);
}

void QueueTasks::AddTask(Task *task)
{
	QueueTask.push(task);
}

Task * QueueTasks::GetAndDeleteTask()
{
	Task *temp;
	if (QueueTask.empty())
		return NULL;
	else
	{
		temp = QueueTask.front();
		QueueTask.pop();
		return temp;
	}
}


THREADPOOL::THREADPOOL(int N, void *ptrFunc)
{
	HANDLE temp;
	hThreads = new std::queue<HANDLE>();
    hNumberMutex = CreateMutex(NULL, false, NULL);
    Number = 0; 
	for (int i = 0; i < N; i++)
	{
		temp = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE) ptrFunc, NULL, CREATE_SUSPENDED, NULL);
		hThreads->push(temp);
		++Number;
	}
}

THREADPOOL::~THREADPOOL(void)
{
	while (!hThreads->empty())
		hThreads->pop();
	delete hThreads;
}

void THREADPOOL::SetQueueTasks(QueueTasks *queueTask)
{
	QueueTask = queueTask;
}

void THREADPOOL::ProcessTask()
{
    while (hThreads->front() != NULL)
    {
		ResumeThread(hThreads->front());
		hThreads->pop();
		--Number;
    }
}

void THREADPOOL::AddThread(void * ptrFunc)
{
     WaitForSingleObject(hNumberMutex, INFINITE);
     HANDLE temp = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE) ptrFunc, NULL, CREATE_SUSPENDED, NULL); 
     hThreads->push(temp); 
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
