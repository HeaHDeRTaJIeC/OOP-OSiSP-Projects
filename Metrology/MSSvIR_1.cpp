
#include "stdafx.h"
#include <iostream>
#include <fstream>
#include <string>
#include <sstream>
#include <iterator>
#include <vector>

#include <regex>

using namespace std;

string modifire[] = {"private", "public", "protected", "internal", "abstract", "async", "const", "event",
					 "extern", "new", "override", "partial", "readonly", "sealed", "static", "unsafe", 
					 "virtual", "volatile"};

string types[] = { "int", "float", "double", "string", "char", "var" };

vector<string> file_lines;


string ReadFile(string fname)
{
	string out;
	string buf;
	bool isCommentStart = false;

	ifstream inf(fname, ios::in);

	do
	{
		getline(inf, buf);
		file_lines.push_back(buf);

		if (buf.find("//") == (size_t)(-1) && buf.find("/*") == (size_t)(-1) && !isCommentStart)
			out += buf + "\n";
		else 
			if (buf.find("/*") != (size_t)(-1) && !isCommentStart)
			{
				out += buf.substr(0, (int)(buf.find("/*")));
				if ( buf.find("*/") == (size_t)(-1) )
				{
					isCommentStart = true;
					out += "\n";
				}
				else
					out += buf.substr(buf.find("*/")+2) + "\n";
			}
			else
				if (buf.find("*/") != (size_t)(-1) && isCommentStart) {
					isCommentStart = false;
					out += buf.substr(buf.find("*/") + 2) + "\n";
				}

	} while (!inf.eof());

	inf.close();

	return out;
}

vector<string> GetSplitString(string str)
{
	vector<string> tokens;

	string buf;
	stringstream ss(str);

	while (ss >> buf)
		tokens.push_back(buf);

	return tokens;
}

void GetModulesNames(vector<string> &modules)
{
	vector<string>::iterator iterator = file_lines.begin();

	smatch m; 

	while (iterator != file_lines.end())
	{
		if (regex_search(*iterator, m, regex("\\busing (\\w+(\.)*)+;")))
		{
			string str = (*iterator);
			while (regex_search(str, m, regex("\\busing (\\w+(\.)*)+;")))
			{
				modules.push_back(m[1]);
				str = m.suffix().str();
			}
		}
		iterator++;
	}
}

void PrintVectors(vector<string> vect)
{
	vector<string>::iterator iterator = vect.begin();;

	while (iterator != vect.end()) {
		cout << *iterator++ << endl;
	}
}

void FindVariableInLine(string str, vector<string> &vars)
{
	string s("int j = kernel[i].NewKernels(kernel[i].FullClass);");
	smatch m;
	regex e("((public|private|protected) |)(int|float|double|var|string|char \\*|\\w+)(\\[\\] | )\\b(\\w*)(| = (\\d*|\"\\w*\")|\\[(\\d*| |)\\]| = new \\w*( \\* |)(\\[(\\d*|)\\]|));");

	if (!regex_search(str, m, regex("using \\w+;")))
	{
		while (regex_search(str, m, e)) {
			/*for (auto x : m)
				cout << x << "|" << endl;*/
			//cout << "Var: " << m[5] << " string: " << str << endl;
			vars.push_back(m[5]);
			str = m.suffix().str();
		}
	}
}

void FindAllVariables(vector<string> &vars)
{
	vector<string>::iterator iterator = file_lines.begin();

	while (iterator != file_lines.end()) {
		FindVariableInLine(*iterator, vars);
		iterator++;
	}
}

int _tmain(int argc, _TCHAR* argv[])
{
	//string text = ReadFile("file.txt");
	//vector<string> tokens = GetSplitString(text);
	//vector<string> modules;
	//vector<string> vars;

	//FindAllVariables(vars);
	//GetModulesNames(modules);

	////cout << "\nWords:" << endl;
	////PrintVectors(tokens);
	//cout << "\nModules:" << endl;
	//PrintVectors(modules);
	//cout << "\nVars:" << endl;
	//PrintVectors(vars);
	//cout << endl;

	string s("int j = kernel[i].NewKernels(kernel[i].FullClass);");
	smatch m;
	regex e("((public|private|protected) |)(int|float|double|var|string|char \\*|\\w+)(\\[\\] | )\\b(\\w*)(| = (\\d*|\"\\w*\")|\\[(\\d*| |)\\]| = new \\w*( \\* |)(\\[(\\d*|)\\]|));");

	if (!regex_search(s, m, regex("using \\w+;")))
	{
		while (regex_search(s, m, e)) {
			for (auto x : m)
				cout << x << "|" << endl;
			cout << "Var: " << m[5] << endl;// " string: " << s << endl;
			s = m.suffix().str();
		}
	}

	system("pause");
	return 0;
}

