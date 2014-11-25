// Metrology1.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <fstream>
#include <string>
#include <sstream>
#include <iterator>
#include <vector>
#include <regex>


//string modifire[] = {"private", "public", "protected", "internal", "abstract", "async", "const", "event",
//					 "extern", "new", "override", "partial", "readonly", "sealed", "static", "unsafe", 
//					 "virtual", "volatile"};
//


std::vector<std::string> FileLines;


//string ReadFile(string fname)
//{
//	string out;
//	string buf;
//	bool isCommentStart = false;
//
//	ifstream inf(fname, ios::in);
//
//	do
//	{
//		getline(inf, buf);
//		file_lines.push_back(buf);
//
//		if (buf.find("//") == (size_t)(-1) && buf.find("/*") == (size_t)(-1) && !isCommentStart)
//			out += buf + "\n";
//		else 
//			if (buf.find("/*") != (size_t)(-1) && !isCommentStart)
//			{
//				out += buf.substr(0, (int)(buf.find("/*")));
//				if ( buf.find("*/") == (size_t)(-1) )
//				{
//					isCommentStart = true;
//					out += "\n";
//				}
//				else
//					out += buf.substr(buf.find("*/")+2) + "\n";
//			}
//			else
//				if (buf.find("*/") != (size_t)(-1) && isCommentStart) 
//				{
//					isCommentStart = false;
//					out += buf.substr(buf.find("*/") + 2) + "\n";
//				}
//
//	} while (!inf.eof());
//
//	inf.close();
//
//	return out;
//}
//
//vector<string> GetSplitString(string str)
//{
//	vector<string> tokens;
//
//	string buf;
//	stringstream ss(str);
//
//	while (ss >> buf)
//		tokens.push_back(buf);
//
//	return tokens;
//}
//
//void GetModulesNames(vector<string> &modules)
//{
//	vector<string>::iterator iterator = file_lines.begin();
//
//	smatch m; 
//
//	while (iterator != file_lines.end())
//	{
//		if (regex_search(*iterator, m, regex("\\busing (\\w+(\.)*)+;")))
//		{
//			string str = (*iterator);
//			while (regex_search(str, m, regex("\\busing (\\w+(\.)*)+;")))
//			{
//                for (auto x : m)
//                   cout << x << "|" << endl;
//				modules.push_back(m[1]);
//				str = m.suffix().str();
//			}
//		}
//		iterator++;
//	}
//}
//
//void PrintVectors(vector<string> vect)
//{
//	vector<string>::iterator iterator = vect.begin();;
//
//	while (iterator != vect.end()) 
//	{
//		cout << *iterator++ << endl;
//	}
//}
//

void findVariableInLine(std::string, std::vector<std::string> &);
void findAllVariables(std::vector<std::string> &);
std::string readFile(std::string);
std::string deleteStringAndComment(std::string, bool *);
bool isLineEmpty(std::string);
std::string buildRegularForVariable();

int _tmain(int argc, _TCHAR* argv[])
{
	std::string tempText = readFile("text.txt");

	std::vector<std::string> vars;
	std::string test = "public static readonly List<Array<int, int>, Listt<float>> points = ";
	findVariableInLine(test, vars);
	//findAllVariables(vars);
	/*int index = 0;
	while (tempText[index] < 0)
		++index;
	std::string fullText = tempText.substr(index, tempText.length());*/
	//std::cout << tempText << std::endl;

	//string text = ReadFile("file.txt");
	//vector<string> tokens = GetSplitString(text);
	//vector<string> modules;
	
	//GetModulesNames(modules);

	////cout << "\nWords:" << endl;
	////PrintVectors(tokens);
	//cout << "\nModules:" << endl;
	//PrintVectors(modules);
	//cout << "\nVars:" << endl;
	//PrintVectors(vars);
	//cout << endl;

	//string s("private int *i = new int;"/*"int j = kernel[i].NewKernels(kernel[i].FullClass);"*/);
	
	//string *s = new string[6];
	//s[0] = "private int i = new int;";
	//s[1] = "public float f;";
	//s[2] = "var item = new int;";
	//s[3] = "public double d;";
	//s[4] = "public string s\"stroka\";";
	//s[5] = "public char ch;";
	//smatch *m = new smatch[6];
	//regex e("((public|private|protected) |)(int|float|double|var|string|char|\\w+)(| \\* | \\*)(\\[\\] | )\\b(\\w*)(| = (\\d*|\"\\w*\")|\\[(\\d*| |)\\]| = new \\w*( \\* |)(\\[(\\d*|)\\]|));");
	//for (int i = 0; i < 6; i++)
	//{
	//	if (!regex_search(s[i], m[i], regex("using \\w+;")))
	//	{
	//		while (regex_search(s[i], m[i], e)) 
	//		{
	//			/*for (auto x : m)
	//				cout << x << "|" << endl;*/
	//			cout << "Var: " << m[i][6] << endl;// " string: " << s << endl;
	//			s[i] = m[i].suffix().str();
	//		}
	//	}
	//}

	system("pause");
	return 0;
}


std::string readFile(std::string fileName)
{
	std::string result, line;
	bool isComment = false;

	std::ifstream fSource;
	fSource.open(fileName, std::ios::in);
	
	while (!fSource.eof())
	{
		getline(fSource, line);
		
		//check empty lines
		if (isLineEmpty(line))
			line = "";

		//check multiline comment flag
		if (!isComment)
		{
			//check line for strings and comments
			line = deleteStringAndComment(line, &isComment);
		}
		else
		{
			//check line for multiline comment end
			int commentEnd = line.find("*/");
			if (commentEnd > -1)
			{
				isComment = false;
				line = line.substr(commentEnd + 2, line.length());
				line = deleteStringAndComment(line, &isComment);
			}
		}
		if (line != "")
		{
			line += '\n';
			FileLines.push_back(line);
		}
		result += line;

	}
	return result;
}


std::string deleteStringAndComment(std::string textline, bool *isComment)
{
	std::string line = textline;
	std::string temp;

	//delete strings in code
	int stringEnd = 0;
	int stringStart = line.find('\"');
	while (stringStart > 0)
	{
		temp += line.substr(stringEnd, stringStart);
		line = line.substr(stringStart + 1, line.length());
		stringEnd = line.find('\"');
		line = line.substr(stringEnd + 1, line.length());
		stringStart = line.find('\"');
		stringEnd = 0;
	}
	temp += line;

	//delete line comments in code
	int commentLine;
	if ((commentLine = temp.find("//")) > -1)
		temp = temp.substr(0, commentLine);

	//delete multiline comments in code
	line = temp;
	temp = "";
	int commentEnd = 0;
	int commentStart = line.find("/*");
	while (commentStart > 0)
	{
		temp += line.substr(0, commentStart);
		line = line.substr(commentStart + 2, line.length());
		commentEnd = line.find("*/");
		if (commentEnd > -1)
		{
			line = line.substr(commentEnd + 2, line.length());
			commentStart = line.find("/*");
		}
		else
		{
			commentStart = -1;
			*isComment = true;
		}
	}
	if (!*isComment)
		temp += line;
	return temp;
}

bool isLineEmpty(std::string textLine)
{
	int lineLength = textLine.length();
	if (lineLength != 0)
		for (int i = 0; i < lineLength; i++)
			if (textLine[i] != ' ')
				return false;
	return true;
}

void findAllVariables(std::vector<std::string> &vars)
{
	std::vector<std::string>::iterator iterator = FileLines.begin();

	while (iterator != FileLines.end()) 
	{
		std::cout << *iterator << std::endl;
		//findVariableInLine(*iterator, vars);
		iterator++;
	}
}

void findVariableInLine(std::string textLine, std::vector<std::string> &vars)
{
	//string s("int j = kernel[i].NewKernels(kernel[i].FullClass);");
	std::smatch m;
	//std::regex e("((public|private|protected|) |)"+AllTypes+"(\\[\\] | )\\b(\\w*)(| = (\\d*|\"\\w*\"|\\w*)|\\[(\\d*| |)\\]| = new \\w*( \\* |)(\\[(\\d*|)\\]|));");
	std::regex newreg(buildRegularForVariable());
	std::ofstream fDest("out.txt", std::ios::out);
	if (!std::regex_search(textLine, m, std::regex("using \\w+;")))
	{
		if (std::regex_search(textLine, m, newreg)) 
		{
			for (auto x : m)
			{
				std::cout << x << "|" << std::endl;
				fDest << x << "|" << std::endl;
			}
			std::cout << "Var: " << m[5] << " string: " << textLine << std::endl;
			vars.push_back(m[5]);
			textLine = m.suffix().str();
			fDest.close();
			//int a;
			//std::cin >> a;
		}
	}
}

std::string buildRegularForVariable()
{
	std::string simpleType = "(\\w+)";
	std::string simpleTypes = "(\\w+)((\\<"+simpleType+"(\\, "+simpleType+")*\\>)|)";
	std::string complexType = "(\\w+)((\\<"+simpleTypes+"(\\, "+simpleTypes+")*\\>)|)";
	std::string allTypes = "(int|float|double|string|char|var|";
	allTypes += (complexType + ")");
	std::string allModifiers = "(public |private |protected |internal |)(static |)(readonly |volatile |)(extern |)"; 
	std::string simpleStructType = "((\\w+)((\\.\\w+)*))";
	std::string	structType = "((\\w+)(\\."+simpleStructType+"(\\((("+simpleStructType+"|("+simpleStructType+"(\\,|))*)|)\\))|))";
	std::string simpleVariable = "(\\[\\]|) " + simpleType;
	std::string variable = "(\\[\\]|) "+structType;
	std::string leftSide = allModifiers + allTypes + simpleVariable + "( = |\\;)";
	return leftSide;
	std::regex e("((public|private|protected|) |)"+allTypes+"(\\[\\] | )\\b(\\w*)(| = (\\d*|\"\\w*\"|\\w*)|\\[(\\d*| |)\\]| = new \\w*( \\* |)(\\[(\\d*|)\\]|));");
	//return;  
}