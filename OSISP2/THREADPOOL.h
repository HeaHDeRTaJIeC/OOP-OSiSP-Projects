#include <windows.h>
#include <queue>

class Task
{
public:
	Task();
	Task(long K);
	~Task();
	void SetTask(long K);
	long GetTask();
private:
	long Milisec;
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
	void AddThread(void *ptrFunc);
	void RemoveThread();
	int GetNumber();
	
	QueueTasks *QueueTask;
private:
    HANDLE hNumberMutex;
    int Number;
	std::queue<HANDLE> *hThreads;

};
