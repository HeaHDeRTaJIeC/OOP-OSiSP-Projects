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
		bool IsEmpty();

		HANDLE hQueueMutex;
	private:
		int Count;
		std::queue<Task *> *QueueTask;
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
		HANDLE hThreadMutex;
	private:
		HANDLE hNumberMutex;
		int Number;
		HANDLE *hThreads;
};
