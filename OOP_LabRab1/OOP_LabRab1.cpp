// OOP_LabRab1.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include "OOP_LabRab1.h"
#include "SHAPES.h"
#include "TRIANGLES.h"
#include "RECTANGLES.h"
#include "LINES.h"
#include "ELLIPSES.h"
#include "LISTS.h"

#define MAX_LOADSTRING 100

// Global Variables:
HINSTANCE hInst;								// current instance
TCHAR szTitle[MAX_LOADSTRING];					// The title bar text
TCHAR szWindowClass[MAX_LOADSTRING];			// the main window class name

// Forward declarations of functions included in this code module:
ATOM				MyRegisterClass(HINSTANCE hInstance);
BOOL				InitInstance(HINSTANCE, int);
LRESULT CALLBACK	WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK	About(HWND, UINT, WPARAM, LPARAM);
void                PaintAll(HWND);

Lists listBody, listTrucks, listBoom, listCross;
bool flag = false;

    Triangles 
		boom(800, 140, 1000, 155, 1000, 125);
    Rectangles 
		body(100, 200, 600, 350), 
		head(200, 100, 450, 200),
		gun(450, 125, 750, 150),
	    cross1(300, 115, 320, 185),
		cross2(270, 140, 350, 160);
	Lines 
		truck1(150, 300, 550, 300),
	    truck2(150, 400, 550, 400);
	Ellipses 
		katok1(100, 300, 200, 400),
		katok2(200, 300, 300, 400),
		katok3(300, 300, 400, 400),
		katok4(400, 300, 500, 400),
		katok5(500, 300, 600, 400);


int APIENTRY _tWinMain(_In_ HINSTANCE hInstance,
                     _In_opt_ HINSTANCE hPrevInstance,
                     _In_ LPTSTR    lpCmdLine,
                     _In_ int       nCmdShow)
{
	UNREFERENCED_PARAMETER(hPrevInstance);
	UNREFERENCED_PARAMETER(lpCmdLine);

	

	listBody.AddNode(&body);
	listBody.AddNode(&head);
	listBody.AddNode(&gun);
	listTrucks.AddNode(&katok1);
	listTrucks.AddNode(&katok2);
	listTrucks.AddNode(&katok3);
	listTrucks.AddNode(&katok4);
	listTrucks.AddNode(&katok5);
	listTrucks.AddNode(&truck1);
	listTrucks.AddNode(&truck2);
	listCross.AddNode(&cross1);
	listCross.AddNode(&cross2);
	listBoom.AddNode(&boom);

 	// TODO: Place code here.
	MSG msg;
	HACCEL hAccelTable;

	// Initialize global strings
	LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
	LoadString(hInstance, IDC_OOP_LABRAB1, szWindowClass, MAX_LOADSTRING);
	MyRegisterClass(hInstance);

	// Perform application initialization:
	if (!InitInstance (hInstance, nCmdShow))
	{
		return FALSE;
	}

	hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_OOP_LABRAB1));

	// Main message loop:
	while (GetMessage(&msg, NULL, 0, 0))
	{
		if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

	return (int) msg.wParam;
}



//
//  FUNCTION: MyRegisterClass()
//
//  PURPOSE: Registers the window class.
//
ATOM MyRegisterClass(HINSTANCE hInstance)
{
	WNDCLASSEX wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style			= CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc	= WndProc;
	wcex.cbClsExtra		= 0;
	wcex.cbWndExtra		= 0;
	wcex.hInstance		= hInstance;
	wcex.hIcon			= LoadIcon(hInstance, MAKEINTRESOURCE(IDI_OOP_LABRAB1));
	wcex.hCursor		= LoadCursor(NULL, IDC_ARROW);
	wcex.hbrBackground	= (HBRUSH)(COLOR_WINDOW+1);
	wcex.lpszMenuName	= MAKEINTRESOURCE(IDC_OOP_LABRAB1);
	wcex.lpszClassName	= szWindowClass;
	wcex.hIconSm		= LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassEx(&wcex);
}

//
//   FUNCTION: InitInstance(HINSTANCE, int)
//
//   PURPOSE: Saves instance handle and creates main window
//
//   COMMENTS:
//
//        In this function, we save the instance handle in a global variable and
//        create and display the main program window.
//
BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
   HWND hWnd;

   hInst = hInstance; // Store instance handle in our global variable

   hWnd = CreateWindow(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
      CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, NULL, NULL, hInstance, NULL);

   if (!hWnd)
   {
      return FALSE;
   }

   ShowWindow(hWnd, nCmdShow);
   UpdateWindow(hWnd);

   return TRUE;
}

//
//  FUNCTION: WndProc(HWND, UINT, WPARAM, LPARAM)
//
//  PURPOSE:  Processes messages for the main window.
//
//  WM_COMMAND	- process the application menu
//  WM_PAINT	- Paint the main window
//  WM_DESTROY	- post a quit message and return
//
//
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	int wmId, wmEvent;
	PAINTSTRUCT ps;
	HDC hdc;

	switch (message)
	{
	case WM_COMMAND:
		wmId    = LOWORD(wParam);
		wmEvent = HIWORD(wParam);
		// Parse the menu selections:
		switch (wmId)
		{
		case IDM_ABOUT:
			DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
			break;
		case ID_DRAW_ALL:
			flag = true;
			PaintAll(hWnd);
			break;
		case ID_DELETE_ALL:
			flag = false;
			UpdateWindow(hWnd);
			break;
		case IDM_EXIT:
			DestroyWindow(hWnd);
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
		break;
	case WM_PAINT:
		hdc = BeginPaint(hWnd, &ps);
		if (flag)
		   PaintAll(hWnd);
		EndPaint(hWnd, &ps);
		break;
	case WM_DESTROY:
		PostQuitMessage(0);
		break;
	default:
		return DefWindowProc(hWnd, message, wParam, lParam);
	}
	return 0;
}

void PaintAll(HWND hWnd)
{
	HBRUSH brush;
	HDC hdc = GetDC(hWnd);
	brush = CreateSolidBrush(RGB(50, 50, 50));
	SelectObject(hdc, brush);
	listBody.DrawList(hdc);
	DeleteObject(brush);

	brush = CreateSolidBrush(RGB(30, 30, 30));
	SelectObject(hdc, brush);
	listTrucks.DrawList(hdc);
	DeleteObject(brush);

    brush = CreateSolidBrush(RGB(255, 30, 30));
	SelectObject(hdc, brush);
	listBoom.DrawList(hdc);
	DeleteObject(brush);

	brush = CreateSolidBrush(RGB(0, 0, 0));
	SelectObject(hdc, brush);
	listCross.DrawList(hdc);
	DeleteObject(brush);
}

// Message handler for about box.
INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	switch (message)
	{
	case WM_INITDIALOG:
		return (INT_PTR)TRUE;

	case WM_COMMAND:
		if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
		{
			EndDialog(hDlg, LOWORD(wParam));
			return (INT_PTR)TRUE;
		}
		break;
	}
	return (INT_PTR)FALSE;
}
