// LabRab1.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include "LabRab1.h"

#define MAX_LOADSTRING 100
#define USER_WIDTH 1000
#define USER_HEIGTH 500
#define OFFSET_STEP 10

// Global Variables:
HINSTANCE hInst;								// current instance
TCHAR szTitle[MAX_LOADSTRING];					// The title bar text
TCHAR szWindowClass[MAX_LOADSTRING];			// the main window class name
HDC StaticDC,                                   // static buffer for pictures
	DynamicDC,
	TextDC,
	UserDC,
	MetaFileDC,
	PrintDC;
HBITMAP StaticBM, 
	DynamicBM, 
	TextBM, 
	UserBM,
	MetaFileBM,
	PrintBM;
HPEN CurrentPen, InvisiblePen, TransparentPen, ErasePen;
HBRUSH CurrentBrush, InvisibleBrush, TransparentBrush, EraseBrush;
HFONT TextFont;
HENHMETAFILE MetaFile;
RECT MetaRect;
TCHAR *OutputText;
COLORREF PenColor = RGB(0, 0, 0);
COLORREF BrushColor = RGB(255, 255, 255);
COLORREF BackGroundColor = RGB(255, 255, 255);
COLORREF TextColor = RGB(0, 0, 0);
COLORREF TransparentColorText = RGB(255, 255, 255); 
COLORREF TransparentColorDynamic;
DOCINFO docInfo;
CString TextFace;
TCHAR key;
TCHAR *keyPtr;
POINT *PolygonPoints;

bool MousePressed = false;
bool TextPressed = false;
bool CtrlPressed = false;
bool ShiftPressed = false;
bool DrawAllowed = true;
static int x, y, x0, y0;
static int xPosition = 0;
static int	yPosition = 0;
static double Zoom = 1;
int type = 0;
int PenSize = 1;
int TextLength = 1;
int PanRight = 0;
int PanDown = 0;
int Index = 0;



// Forward declarations of functions included in this code module:
ATOM				MyRegisterClass(HINSTANCE hInstance);
BOOL				InitInstance(HINSTANCE, int);
LRESULT CALLBACK	WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK	About(HWND, UINT, WPARAM, LPARAM);
void                PrepareDC();

int APIENTRY _tWinMain(_In_ HINSTANCE hInstance,
                     _In_opt_ HINSTANCE hPrevInstance,
                     _In_ LPTSTR    lpCmdLine,
                     _In_ int       nCmdShow)
{
	UNREFERENCED_PARAMETER(hPrevInstance);
	UNREFERENCED_PARAMETER(lpCmdLine);

 	// TODO: Place code here.
	MSG msg;
	HACCEL hAccelTable;


	// Initialize global strings
	LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
	LoadString(hInstance, IDC_LABRAB1, szWindowClass, MAX_LOADSTRING);
	MyRegisterClass(hInstance);

	// Perform application initialization:
	if (!InitInstance (hInstance, nCmdShow))
	{
		return FALSE;
	}


	hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_LABRAB1));

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
	wcex.hIcon			= LoadIcon(hInstance, MAKEINTRESOURCE(IDI_LABRAB1));
	wcex.hCursor		= LoadCursor(NULL, IDC_ARROW);
	wcex.hbrBackground	= (HBRUSH)(COLOR_WINDOW+1);
	wcex.lpszMenuName	= MAKEINTRESOURCE(IDC_LABRAB1);
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

   HDC hdc = GetDC(hWnd);
   StaticDC = CreateCompatibleDC(hdc);
   StaticBM = CreateCompatibleBitmap(hdc, USER_WIDTH, USER_HEIGTH);
   SelectObject(StaticDC, StaticBM);
   InvisiblePen = CreatePen(PS_SOLID, PenSize, BackGroundColor);
   SelectObject(StaticDC, InvisiblePen);
   InvisibleBrush = CreateSolidBrush(BackGroundColor);
   SelectObject(StaticDC, InvisibleBrush);
   Rectangle(StaticDC, 0, 0, USER_WIDTH, USER_HEIGTH);
   DeleteObject(InvisiblePen);
   DeleteObject(InvisibleBrush);

   DynamicDC = CreateCompatibleDC(hdc);
   DynamicBM = CreateCompatibleBitmap(hdc, USER_WIDTH, USER_HEIGTH);
   SelectObject(DynamicDC, DynamicBM);

   TextDC = CreateCompatibleDC(hdc);
   TextBM = CreateCompatibleBitmap(hdc, USER_WIDTH, USER_HEIGTH);
   SelectObject(TextDC, TextBM);
   InvisiblePen = CreatePen(PS_SOLID, PenSize, BackGroundColor);
   SelectObject(TextDC, InvisiblePen);
   InvisibleBrush = CreateSolidBrush(BackGroundColor);
   SelectObject(TextDC, InvisibleBrush);
   Rectangle(TextDC, 0, 0, USER_WIDTH, USER_HEIGTH);
   DeleteObject(InvisiblePen);
   DeleteObject(InvisibleBrush);

   UserDC = CreateCompatibleDC(hdc);
   UserBM = CreateCompatibleBitmap(hdc, USER_WIDTH, USER_HEIGTH);
   SelectObject(UserDC, UserBM);
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
	POINT *Triangle;
	TCHAR szFilter[] = _T("Paint file (*.emf)|*.emf|All files (*.*)|*.*||");
	CFileDialog dlgFileOpen(TRUE, NULL, _T("*.emf"), OFN_FILEMUSTEXIST | OFN_HIDEREADONLY, szFilter);
	CFileDialog dlgFileSave(FALSE, _T("Unnamed"), _T("*.emf"), OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT, szFilter);
	PRINTDLG dlgPrint;
	CFontDialog dlgFont;
	CFontDialog *dlgFontPtr = &dlgFont;
	CColorDialog dlgColor;
	CString fileName;
	
	switch (message)
	{
	case WM_RBUTTONDOWN:
	{
		if (DrawAllowed)
		{
			if (type == 2)
			{
				TransparentBlt(StaticDC, 0, 0, USER_WIDTH, USER_HEIGTH, DynamicDC, 0, 0, USER_WIDTH, USER_HEIGTH, TransparentColorDynamic);
				MousePressed = false;
			}
			if (type == 8)
			{
				CurrentPen = CreatePen(PS_SOLID, PenSize, PenColor);
				SelectObject(StaticDC, CurrentPen);
				CurrentBrush = CreateSolidBrush(BrushColor);
				SelectObject(StaticDC, CurrentBrush);
				Polygon(StaticDC, PolygonPoints, Index);
				Index = 0;
				MousePressed = false;
			}
		}
		break;
	}
	case WM_LBUTTONDOWN:
	{
		if (DrawAllowed)
		{
			if (type != 2 && type != 8)
			{
				x0 = LOWORD(lParam);
				y0 = HIWORD(lParam);
				x = x0;
				y = y0;
				if (type == 7)
					if (TextPressed)
					{
						TextLength = 1;
						OutputText = (TCHAR*) realloc(OutputText, sizeof(TCHAR)*TextLength);
						OutputText[TextLength-1] = '\0';
						TransparentBlt(StaticDC, 0, 0, USER_WIDTH, USER_HEIGTH, TextDC, 0, 0, USER_WIDTH, USER_HEIGTH, TransparentColorText);
					}
					else
						TextPressed = true;
				else
					MousePressed = true;
			}
			else
			{
				if (MousePressed)
				{
					TransparentBlt(StaticDC, 0, 0, USER_WIDTH, USER_HEIGTH, DynamicDC, 0, 0, USER_WIDTH, USER_HEIGTH, TransparentColorDynamic);
					x0 = LOWORD(lParam);
					y0 = HIWORD(lParam);
					Index++;
					PolygonPoints = (POINT *) realloc(PolygonPoints, sizeof(POINT) * Index);
					PolygonPoints[Index - 1].x = x0;
					PolygonPoints[Index - 1].y = y0;
				}
				else
				{
					MousePressed = true;
					x0 = LOWORD(lParam);
					y0 = HIWORD(lParam);
					Index++;
					PolygonPoints = (POINT *) malloc (sizeof(POINT) * Index);
					PolygonPoints[Index - 1].x = x0;
					PolygonPoints[Index - 1].y = y0;
				}
			}
			InvalidateRect(hWnd, NULL, false);
		}
		break;
	}
	case WM_LBUTTONUP:
	{
		if (DrawAllowed)
		{
			if (type != 2 && type !=7 && type != 8)
				MousePressed = false;
			else
			{
				x0 = LOWORD(lParam);
				y0 = HIWORD(lParam);
				Index++;
				PolygonPoints = (POINT *) realloc(PolygonPoints, sizeof(POINT) * Index);
				PolygonPoints[Index - 1].x = x0;
				PolygonPoints[Index - 1].y = y0;
			}
			if (type != 6)
				TransparentBlt(StaticDC, 0, 0, USER_WIDTH, USER_HEIGTH, DynamicDC, 0, 0, USER_WIDTH, USER_HEIGTH, TransparentColorDynamic);
		}
		break;
	}
	case WM_MOUSEMOVE:
	{
		if (DrawAllowed)
		{
			if (type != 7)
			{
				TransparentBrush = CreateSolidBrush(TransparentColorDynamic);
				SelectObject(DynamicDC, TransparentBrush);
				TransparentPen = CreatePen(PS_SOLID, PenSize, TransparentColorDynamic);
				SelectObject(DynamicDC, TransparentPen);
				Rectangle(DynamicDC, 0, 0, USER_WIDTH, USER_HEIGTH);
				DeleteObject(TransparentBrush);
				DeleteObject(TransparentPen);

				InvalidateRect(hWnd, NULL, false);

				CurrentBrush = CreateSolidBrush(BrushColor);
				CurrentPen = CreatePen(PS_SOLID, PenSize, PenColor);

				if (MousePressed)
					switch (type)
					{
					case 0:
						SelectObject(StaticDC, CurrentPen);
						MoveToEx(StaticDC, x, y, NULL);
						x = LOWORD(lParam);
						y = HIWORD(lParam);
						LineTo(StaticDC, x, y);
						break;
					case 1:
						SelectObject(DynamicDC, CurrentPen);
						MoveToEx(DynamicDC, x0, y0, NULL);
						LineTo(DynamicDC, LOWORD(lParam), HIWORD(lParam));
						break;
					case 2:
						SelectObject(DynamicDC, CurrentPen);
						MoveToEx(DynamicDC, x0, y0, NULL);
						LineTo(DynamicDC, LOWORD(lParam), HIWORD(lParam));
						break;
					case 3:
						SelectObject(DynamicDC, CurrentPen);
						SelectObject(DynamicDC, CurrentBrush);

						Triangle = new POINT[3];
						Triangle[0].x = (x0 + LOWORD(lParam))/2;
						Triangle[0].y = y0;
						Triangle[1].x = x0;
						Triangle[1].y = HIWORD(lParam);
						Triangle[2].x = LOWORD(lParam);
						Triangle[2].y = HIWORD(lParam);
				
						Polygon(DynamicDC, Triangle, 3);
						delete [] Triangle;
						break;
					case 4:
						SelectObject(DynamicDC, CurrentPen);
						SelectObject(DynamicDC, CurrentBrush);
						Rectangle(DynamicDC, x0, y0, LOWORD(lParam), HIWORD(lParam));
						break;
					case 5:
						SelectObject(DynamicDC, CurrentPen);
						SelectObject(DynamicDC, CurrentBrush);
						Ellipse(DynamicDC, x0, y0, LOWORD(lParam), HIWORD(lParam));
						break;
					case 6:
						InvisiblePen = CreatePen(PS_SOLID, 10, BackGroundColor);
						SelectObject(StaticDC, InvisiblePen);
						ErasePen = CreatePen(PS_SOLID, 1, RGB(0, 0, 0));
						EraseBrush = CreateSolidBrush(RGB(255, 255, 255));
						SelectObject(DynamicDC, ErasePen);
						SelectObject(DynamicDC, EraseBrush);
						Rectangle(DynamicDC, LOWORD(lParam) - 5, HIWORD(lParam) - 5, LOWORD(lParam) + 5, HIWORD(lParam) + 5);
						MoveToEx(StaticDC, x, y, NULL);
						x = LOWORD(lParam);
						y = HIWORD(lParam);
						LineTo(StaticDC, x, y);
						break;
					case 8:
						SelectObject(DynamicDC, CurrentPen);
						MoveToEx(DynamicDC, x0, y0, NULL);
						LineTo(DynamicDC, LOWORD(lParam), HIWORD(lParam));
						break;
					default:
						break;
					}
				else
					if (type == 6)
					{
						ErasePen = CreatePen(PS_SOLID, 1, RGB(0, 0, 0));
						EraseBrush = CreateSolidBrush(RGB(255, 255, 255));
						SelectObject(DynamicDC, ErasePen);
						SelectObject(DynamicDC, EraseBrush);
						Rectangle(DynamicDC, LOWORD(lParam) - 5, HIWORD(lParam) - 5, LOWORD(lParam) + 5, HIWORD(lParam) + 5);
					}
			}
			else
			{
				PrepareDC();
				InvalidateRect(hWnd, NULL, false);
			}
			DeleteObject(CurrentBrush);
			DeleteObject(CurrentPen);
		}
		break;
	}
	case WM_MOUSEWHEEL:
		DrawAllowed = false;
		if (CtrlPressed)
		{
			if (GET_WHEEL_DELTA_WPARAM(wParam) < 0)
				if (Zoom < 2)
					Zoom += 0.1;
			if (GET_WHEEL_DELTA_WPARAM(wParam) > 0)
				if (Zoom > 0.1)
					Zoom -= 0.1;
		}
		else
			if (ShiftPressed)
				if (GET_WHEEL_DELTA_WPARAM(wParam) < 0)
				{
					xPosition += (int) (OFFSET_STEP * Zoom);
					PanRight++;
				}
				else
				{
					xPosition -= (int) (OFFSET_STEP * Zoom);
					PanRight--;
				}
			else
				if (GET_WHEEL_DELTA_WPARAM(wParam) < 0)
				{
					yPosition -= (int) (OFFSET_STEP * Zoom);
					PanDown++;
				}
				else
				{
					yPosition += (int) (OFFSET_STEP * Zoom);
					PanDown--;
				}
		InvalidateRect(hWnd, NULL, true);
		break;
	case WM_KEYDOWN:
		switch(wParam)
		{
			case VK_CONTROL:
				CtrlPressed = true;
				break;
			case VK_SHIFT:
				ShiftPressed = true;
				break;
			default:
				break;
		}
		break;
	case WM_KEYUP:
		switch(wParam)
		{
			case VK_CONTROL:
				CtrlPressed = false;
				break;
			case VK_SHIFT:
				ShiftPressed = false;
				break;
			default:
				break;
		}
		break;
	case WM_CHAR:
		if (DrawAllowed)
		{
			if (TextPressed)
			{
				switch(wParam)
				{
				case 0x08:
					if (TextLength > 1)
					{
						TransparentPen = CreatePen(PS_SOLID, 1, TransparentColorText);
						SelectObject(TextDC, TransparentPen);
						TransparentBrush = CreateSolidBrush(TransparentColorText);
						SelectObject(TextDC, TransparentBrush);
						Rectangle(TextDC, 0, 0, USER_WIDTH, USER_HEIGTH);
						DeleteObject(TransparentPen);
						DeleteObject(TransparentBrush);
						TextLength--;
						OutputText = (TCHAR *) realloc(OutputText, sizeof(TCHAR)*TextLength);
						OutputText[TextLength-1] = '\0';
						TextOut(TextDC, x0, y0, OutputText, TextLength-1);
					}
					break;
				case 0x0D:
					TransparentBlt(StaticDC, 0, 0, USER_WIDTH, USER_HEIGTH, TextDC, 0, 0, USER_WIDTH, USER_HEIGTH, TransparentColorText);
					TextLength = 1;
					OutputText = (TCHAR *) realloc(OutputText, sizeof(TCHAR)*TextLength);
					OutputText[TextLength-1] = '\0';
					TextPressed = false;
					break;
				case 0x1B:
					TextPressed = false;
					TransparentPen = CreatePen(PS_SOLID, 1, TransparentColorText);
					SelectObject(TextDC, TransparentPen);
					TransparentBrush = CreateSolidBrush(TransparentColorText);
					SelectObject(TextDC, TransparentBrush);
					Rectangle(TextDC, 0, 0, USER_WIDTH, USER_HEIGTH);
					DeleteObject(TransparentPen);
					DeleteObject(TransparentBrush);
					TextLength = 1;
					OutputText = (TCHAR *) realloc(OutputText, sizeof(TCHAR)*TextLength);
					OutputText[TextLength-1] = '\0';
					break;
				default:
					{
						key = (TCHAR) wParam;
						OutputText[TextLength-1] = key;
						TextLength++;
						OutputText = (TCHAR *) realloc(OutputText, sizeof(TCHAR)*(TextLength));
						OutputText[TextLength-1] = '\0';
						TextOut(TextDC, x0, y0, OutputText, TextLength-1);
					}
					break;
				}
				InvalidateRect(hWnd, NULL, false);
			}
		}
		break;

	case WM_COMMAND:
		wmId    = LOWORD(wParam);
		wmEvent = HIWORD(wParam);
		// Parse the menu selections:
		switch (wmId)
		{
		case IDM_ABOUT:
			DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
			break;
		case IDM_EXIT:
			DestroyWindow(hWnd);
			break;
		case ID_TOOLS_PEN:
			type = 0;
			break;
		case ID_TOOLS_LINE:
			type = 1;
			break;
		case ID_TOOLS_POLYLINE:
			type = 2;
			break;
		case ID_TOOLS_TRIANGLE:
			type = 3;
			break;
		case ID_TOOLS_RECTALGLE:
			type = 4;
			break;
		case ID_TOOLS_ELLIPSE:
			type = 5;
			break;
		case ID_TOOLS_ERASER:
			type = 6;
			break;
		case ID_TOOLS_TEXT:
			type = 7;
			OutputText = (TCHAR *) malloc(sizeof(TCHAR)*TextLength);
			OutputText[TextLength-1] = '\0';
			break;
		case ID_TOOLS_POLYGON:
			type = 8;
			break;
		case ID_TOOLS_ERASEALL:
			type = 9;
			InvisiblePen = CreatePen(PS_SOLID, 1, BackGroundColor);
			InvisibleBrush = CreateSolidBrush(BackGroundColor);
			SelectObject(StaticDC, InvisiblePen);
			SelectObject(StaticDC, InvisibleBrush);
			Rectangle(StaticDC, 0, 0, USER_WIDTH, USER_HEIGTH);
			SelectObject(DynamicDC, InvisiblePen);
			SelectObject(DynamicDC, InvisibleBrush);
			Rectangle(DynamicDC, 0, 0, USER_WIDTH, USER_HEIGTH);
			SelectObject(TextDC, InvisiblePen);
			SelectObject(TextDC, InvisibleBrush);
			Rectangle(TextDC, 0, 0, USER_WIDTH, USER_HEIGTH);
			DeleteObject(InvisiblePen);
			DeleteObject(InvisibleBrush);
			break;
		case ID_PENWIDTH_1:
			PenSize = 1;
			break;
		case ID_PENWIDTH_2:
			PenSize = 2;
			break;
		case ID_PENWIDTH_3:
			PenSize = 3;
			break;
		case ID_PENWIDTH_5:
			PenSize = 5;
			break;
		case ID_PENWIDTH_8:
			PenSize = 8;
			break;
		case ID_PENWIDTH_10:
			PenSize = 10;
			break;
		case ID_SETTINGS_PEN:
			if (dlgColor.DoModal() == IDOK)
			{
				PenColor = dlgColor.GetColor();
				TransparentColorDynamic = RGB((rand() % 254 + 1), (rand() % 254 + 1), (rand() % 254 + 1));
				while ((TransparentColorDynamic == PenColor) || (TransparentColorDynamic == BrushColor))
					TransparentColorDynamic = RGB((rand() % 254 + 1), (rand() % 254 + 1), (rand() % 254 + 1));
			}
			break;
		case ID_SETTINGS_BRUSH:
			if (dlgColor.DoModal() == IDOK)
			{
				BrushColor = dlgColor.GetColor();
				TransparentColorDynamic = RGB((rand() % 254 + 1), (rand() % 254 + 1), (rand() % 254 + 1));
				while ((TransparentColorDynamic == PenColor) || (TransparentColorDynamic == BrushColor))
					TransparentColorDynamic = RGB((rand() % 254 + 1), (rand() % 254 + 1), (rand() % 254 + 1));
			}
			break;
		case ID_SETTINGS_FONT:
			if (dlgFont.DoModal() == IDOK)
			{
				TextFace = dlgFont.GetFaceName();
				TextFont = CreateFont(0, 0, 0, 0, 
					dlgFont.GetWeight(), dlgFont.IsItalic(), dlgFont.IsUnderline(), dlgFont.IsStrikeOut(),
					DEFAULT_CHARSET, OUT_OUTLINE_PRECIS, CLIP_DEFAULT_PRECIS, DEFAULT_QUALITY, VARIABLE_PITCH,
					TextFace);
				TextColor = dlgFont.GetColor();
				SelectObject(TextDC, TextFont);
				SetTextColor(TextDC, TextColor);
				TransparentColorText = RGB(rand() % 254 + 1, rand() % 254 + 1, rand() % 254 + 1);
				while (TransparentColorText == TextColor)
					TransparentColorText = RGB(rand() % 254 + 1, rand() % 254 + 1, rand() % 254 + 1);
				TransparentPen = CreatePen(PS_SOLID, 1, TransparentColorText);
				SelectObject(TextDC, TransparentPen);
				TransparentBrush = CreateSolidBrush(TransparentColorText);
				SelectObject(TextDC, TransparentBrush);
				Rectangle(TextDC, 0, 0, USER_WIDTH, USER_HEIGTH);
				DeleteObject(TransparentPen);
				DeleteObject(TransparentBrush);
			}
			break;
		case ID_MYFILE_OPEN:
			if (dlgFileOpen.DoModal() == IDOK)
			{
				fileName = dlgFileOpen.GetPathName();
				MetaFile = GetEnhMetaFile(fileName);
				MetaRect.left = 0;
				MetaRect.top = 0;
				MetaRect.right = USER_WIDTH;
				MetaRect.bottom = USER_HEIGTH;
				hdc = GetDC(hWnd);
				MetaFileDC = CreateCompatibleDC(hdc);
				MetaFileBM = CreateCompatibleBitmap(hdc, USER_WIDTH, USER_HEIGTH);
				SelectObject(MetaFileDC, MetaFileBM);
				PlayEnhMetaFile(MetaFileDC, MetaFile, &MetaRect);
				BitBlt(StaticDC, 0, 0, USER_WIDTH, USER_HEIGTH, MetaFileDC, 0, 0, SRCCOPY);
				CloseEnhMetaFile(MetaFileDC);
			}
			break;
		case ID_MYFILE_SAVE:
			if (dlgFileSave.DoModal() == IDOK)
			{
				fileName = dlgFileSave.GetPathName();
				MetaFileDC = CreateEnhMetaFile(UserDC, fileName, NULL, NULL);
				PrepareDC();
				BitBlt(MetaFileDC, 0, 0, USER_WIDTH, USER_HEIGTH, UserDC, 0, 0, SRCCOPY);
				CloseEnhMetaFile(MetaFileDC);
			}
			break;
		case ID_MYFILE_PRINT:
			ZeroMemory(&dlgPrint, sizeof(dlgPrint));
			dlgPrint.lStructSize = sizeof(dlgPrint);
			dlgPrint.hwndOwner = hWnd;
			dlgPrint.hDevMode = NULL; 
			dlgPrint.hDevNames = NULL; 
			dlgPrint.Flags = PD_USEDEVMODECOPIESANDCOLLATE | PD_RETURNDC;
			dlgPrint.nCopies = 1;
			dlgPrint.nFromPage = 0xFFFF;
			dlgPrint.nToPage = 0xFFFF;
			dlgPrint.nMinPage = 1;
			dlgPrint.nMaxPage = 0xFFFF;

			if (PrintDlg(&dlgPrint))
			{
				x = GetDeviceCaps(dlgPrint.hDC, LOGPIXELSX);
				y = GetDeviceCaps(dlgPrint.hDC, LOGPIXELSY);
				docInfo.cbSize = sizeof(docInfo);
				docInfo.lpszDocName=L"Print Image";
				docInfo.fwType=NULL;
				docInfo.lpszDatatype=NULL;
				docInfo.lpszOutput=NULL;

				StartDoc(dlgPrint.hDC, &docInfo);
				StartPage(dlgPrint.hDC);
				PrepareDC();
				PatBlt(dlgPrint.hDC, 0,0, USER_WIDTH, USER_HEIGTH, PATCOPY);
				BitBlt(dlgPrint.hDC, 0, 0, USER_WIDTH, USER_HEIGTH, UserDC, 0, 0, SRCCOPY);
				EndPage(dlgPrint.hDC);
				EndDoc(dlgPrint.hDC);
				DeleteDC(dlgPrint.hDC);
			}
			break;
		case ID_PAN_MIDDLE:
			DrawAllowed = true;
			PanRight = 0;
			PanDown = 0;
			Zoom = 1;
			xPosition = 0;
			yPosition = 0;
			InvalidateRect(hWnd, NULL, true);
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
		break;
	case WM_PAINT:
		hdc = BeginPaint(hWnd, &ps);
		PrepareDC();
		StretchBlt(hdc, 0,  0, USER_WIDTH, USER_HEIGTH,
			UserDC, xPosition, yPosition, 
			xPosition + (int) (USER_WIDTH * Zoom), yPosition + (int) (USER_HEIGTH * Zoom), SRCCOPY);
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


void PrepareDC()
{
	InvisibleBrush = CreateSolidBrush(BackGroundColor);
	SelectObject(UserDC, InvisibleBrush);
	InvisiblePen = CreatePen(PS_SOLID, PenSize, BackGroundColor);
	SelectObject(UserDC, InvisiblePen);
	Rectangle(UserDC, 0, 0, USER_WIDTH, USER_HEIGTH);
	DeleteObject(InvisibleBrush);
	DeleteObject(InvisiblePen);
	BitBlt(UserDC, 0, 0, USER_WIDTH, USER_HEIGTH, StaticDC, 0, 0, SRCCOPY);
	TransparentBlt(UserDC, 0, 0, USER_WIDTH, USER_HEIGTH, DynamicDC, 0, 0, USER_WIDTH, USER_HEIGTH, TransparentColorDynamic);
	TransparentBlt(UserDC, 0, 0, USER_WIDTH, USER_HEIGTH, TextDC, 0, 0, USER_WIDTH, USER_HEIGTH, TransparentColorText);
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
