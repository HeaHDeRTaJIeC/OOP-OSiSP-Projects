#include "stdafx.h"
#include "tchar.h"
#include "LISTS.h"


Lists :: Lists()
{
   head = NULL;
}


void Lists :: AddNode(Shapes *CurrentObjPtr)
{
	if (head == NULL)
	{
		head = new Node;
		head->ShapeObjPtr = CurrentObjPtr;
		head->next = NULL;
	}
	else
	{
		Node *tmp = head;
		while (tmp->next != NULL)
			tmp = tmp->next;

		tmp->next = new Node;
		tmp->next->ShapeObjPtr = CurrentObjPtr;
		tmp->next->next = NULL;
	}
}


void Lists :: DrawList(HDC hdc)
{
    Node *tmp = head;
	if (tmp == NULL)
		TextOut(hdc, 5, 5, _T("Spisok is empty"), 15);
	while (tmp != NULL)
	{
		tmp->ShapeObjPtr->Draw(hdc);
		tmp = tmp->next;
	}
}
