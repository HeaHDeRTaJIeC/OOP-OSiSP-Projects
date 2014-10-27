#include "stdafx.h"
#include "THREADPOOL.h"
#include "OSISP2.h"


Task::Task()
{
	Milisec = 1;
}

Task::Task(int K)
{
	Milisec = K;
}

void Task::SetTask(int K)
{
	Milisec = K;
}

int Task::GetTask()
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
	Number = N;
	hThreads = new std::queue<HANDLE>(); 
	for (int i = 0; i < Number; i++)
	{
		temp = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE) ptrFunc, NULL, CREATE_SUSPENDED, NULL);
		hThreads->push(temp);
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
	HANDLE temp;
	for (int i = 0; i < Number; i++)
	{
		temp = hThreads->front();
		ResumeThread(temp);
		hThreads->pop();
	}
}