#pragma once
#include <queue>

class Task
{
public:
	Task();
	Task(int K);
	~Task();
	void SetTask(int K);
	int GetTask();
private:
	int Milisec;
};


class QueueTasks
{
public:
	QueueTasks();
	~QueueTasks();
	void AddTask(Task *task);
	Task *GetAndDeleteTask();
	HANDLE hQueueMutex;
private:
	std::queue<Task *> QueueTask;
};


class THREADPOOL
{
public:
	THREADPOOL(int N, void *ptrFunc);
	~THREADPOOL(void);
	void SetQueueTasks(QueueTasks *queueTask);
	void ProcessTask();
	QueueTasks *QueueTask;
private:
	int Number;
	std::queue<HANDLE> *hThreads;

};