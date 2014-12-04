
#include "stdafx.h"
#include <iostream>
#include <fstream>
#include <string>
#include <sstream>
#include <iterator>
#include <vector>
#include <regex>

struct VariableInfo
{
	std::string methodsName;
	std::string variableName;
	double difficult;
	double numberOfCalls;
	int spen;
};

struct MethodCallsInfo
{
	std::string methodName;
	int calls;
	int called;
};

std::vector<std::string> FileLines;
std::vector<std::string> MethodsText;
std::vector<MethodCallsInfo> MethodsCalls;

std::string readFile(std::string);
std::string deleteStringAndComment(std::string, bool *);
bool isLineEmpty(std::string);

std::string buildLeftSideRegularForVariable();

void findAllVariables(std::vector<VariableInfo> &);
void findVariableInLine(std::string, std::vector<VariableInfo> &, std::string);
void findVariablesInMethod(std::string, std::vector<VariableInfo> &, std::vector<VariableInfo> &, std::string);

void findManageConstructions(std::vector<std::string> &);
void findManageConstruction(std::string, std::vector<std::string> &);

void findMethodText(std::vector<std::string> &);
bool findMethodNames(std::string, std::vector<std::string> &);

void printVector(std::vector<VariableInfo>, bool);
void printVector(std::vector<std::string>);

std::vector<std::string> findMethodsCalls(std::string, std::vector<std::string>);
std::string findCallInMethod(std::string, std::vector<std::string>);

void fillMethodNameStruct(std::vector<std::string> &);

void calculateVariablesDifficulty(std::vector<std::string>, std::vector<VariableInfo> &, std::vector<VariableInfo> &, int);
int calculateMethodCalls(std::vector<std::string>);
void calculateSpenLocal(std::vector<std::string>, std::vector<VariableInfo> &);
void calculateSpenGlobal(std::vector<VariableInfo> &);

int _tmain(int argc, _TCHAR* argv[])
{
	int numberOfMethods;
	std::string tempText = readFile("C#.txt");
	std::vector<std::string> constructs, methodsName;
	std::vector<VariableInfo> globalVariables, localVariables, allVariables;

	//search all variables
	findAllVariables(allVariables);
	globalVariables.resize(allVariables.size());
	std::copy(allVariables.begin(), allVariables.end(), globalVariables.begin());

	//methods names and bodies
	findMethodText(methodsName);
	numberOfMethods = methodsName.size();

	fillMethodNameStruct(methodsName);

	//separate local variables
	std::vector<std::string>::iterator methodTextIterator = MethodsText.begin();
	std::vector<std::string>::iterator methodNameIterator = methodsName.begin();
	while (methodTextIterator != MethodsText.end())
	{
		findVariablesInMethod(*methodTextIterator, globalVariables, localVariables, *methodNameIterator);
		++methodTextIterator;
		++methodNameIterator;
	}


	std::cout << "MacClure" << std::endl;
	calculateVariablesDifficulty(methodsName, globalVariables, localVariables, numberOfMethods);
	printVector(globalVariables, true);
	printVector(localVariables, true);
	std::cout << "M = " << calculateMethodCalls(methodsName) << std::endl; 

	std::cout << "Spen" << std::endl;
	calculateSpenLocal(methodsName, localVariables);
	calculateSpenGlobal(globalVariables);
	printVector(localVariables, false);
	printVector(globalVariables, false);

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

std::string buildLeftSideRegularForVariable()
{
	std::string simpleType = "(\\w+)";
	std::string simpleTypes = "(\\w+)((\\<" + simpleType + "(\\, " + simpleType + ")*\\>)|)";
	std::string complexType = "(\\w+)((\\<" + simpleTypes + "(\\, " + simpleTypes + ")*\\>)|)";
	std::string allTypes = "(int|float|double|string|char|var|";
	allTypes += (complexType + ")");
	std::string allModifiers = "(public |private |protected |internal |)(static |)(readonly |volatile |)(extern |)";
	std::string simpleVariable = "(\\[\\]|) " + simpleType;

	std::string leftSide = allModifiers + allTypes + simpleVariable + "( = |\\;)";
	return leftSide;
}

void findAllVariables(std::vector<VariableInfo> &variables)
{
	std::vector<std::string>::iterator iterator = FileLines.begin();

	//search all variables in whole text
	while (iterator != FileLines.end())
	{
		findVariableInLine(*iterator, variables, "Global");
		iterator++;
	}
}

void findVariableInLine(std::string textLine, std::vector<VariableInfo> &variables, std::string methodName)
{
	std::smatch match;
	std::string leftSideVariable = buildLeftSideRegularForVariable();
	std::regex regularVariable(leftSideVariable);
	//find variables, fill struct fields and push in vector
	if (!std::regex_search(textLine, match, std::regex("using \\w+;")))
	{
		if (std::regex_search(textLine, match, regularVariable))
		{
			VariableInfo temp;
			temp.variableName = match[23];
			temp.difficult = 0.0;
			temp.numberOfCalls = 0.0;
			temp.methodsName = methodName;
			temp.spen = 0;
			variables.push_back(temp);

			textLine = match.suffix().str();
		}
	}
}


void findManageConstructions(std::vector<std::string> &constructios)
{
	std::vector<std::string>::iterator iterator = FileLines.begin();

	//find manage constructions in whole text
	while (iterator != FileLines.end())
	{
		findManageConstruction(*iterator, constructios);
		iterator++;
	}
}

void findManageConstruction(std::string textLine, std::vector<std::string> &constructios)
{
	std::smatch match;

	std::string whileRegular = "( )*(while \\(.*\\))";
	std::string forRegular = "( )*(for \\(.*\\))";
	std::string ifRegular = "( )*(if \\(.*\\))";

	std::regex whileRegex(whileRegular);
	std::regex forRegex(forRegular);
	std::regex ifRegex(ifRegular);

	//find if, for, while constructions
	if (!std::regex_search(textLine, match, std::regex("using \\w+;")))
	{
		if (std::regex_search(textLine, match, ifRegex)) {
			constructios.push_back(match[2]);
		}
		else if (std::regex_search(textLine, match, forRegex)) {
			constructios.push_back(match[2]);
		}
		else if (std::regex_search(textLine, match, whileRegex)) {
			constructios.push_back(match[2]);
		}
	}
}

void findMethodText(std::vector<std::string> &methodNames) 
{
	std::vector<std::string>::iterator iterator = FileLines.begin();

	//find method body and push in vector
	while (iterator != FileLines.end())
	{
		std::string textMethod = "";
		int bracketNumber = 0;
		if (findMethodNames(*iterator, methodNames)) 
		{
			do 
			{
				*iterator++;
				textMethod += *iterator;
				int i = 0;
				char symbol;
				while ((symbol = (*iterator)[i]) != '\n')
				{
					if (symbol == '{') 
						++bracketNumber;
					if (symbol == '}')
						--bracketNumber;
					++i;
				}
			} while (bracketNumber);
		}
		if (!(textMethod.empty())) 
			MethodsText.push_back(textMethod);
		++iterator;
	}
}

bool findMethodNames(std::string textLine, std::vector<std::string> &methodNames)
{
	bool result = false;
	std::smatch match;

	std::string allTypes = "(int |float |double |string |char |void |)";
	std::string allModifiers = "(public |private |protected |internal )(static |)(readonly |volatile |)(extern |)";

	std::string methodNameRegular = allModifiers + allTypes + "(\\w+)(\\(.*\\))";
	std::regex regex(methodNameRegular);

	//find method names and push in vector
	if (!std::regex_search(textLine, match, std::regex("using \\w+;")))
	{
		if (std::regex_search(textLine, match, regex))
		{
			result = true;
			methodNames.push_back(match[6]);
		}
	}
	return result;
}

void printVector(std::vector<VariableInfo> vector, bool flag)
{
	std::vector<VariableInfo>::iterator iterator = vector.begin();

	while (iterator != vector.end())
	{
		if (flag)
			std::cout << (*iterator).methodsName << ": " << (*iterator).variableName << " dif:" << (*iterator).difficult << std::endl;
		else
			std::cout << (*iterator).methodsName << ": " << (*iterator).variableName << " spen:" << (*iterator).spen << std::endl;
		iterator++;
	}
}

void printVector(std::vector<std::string> vector)
{
	std::vector<std::string>::iterator iterator = vector.begin();

	while (iterator != vector.end())
	{
		std::cout << *iterator << std::endl;
		iterator++;
	}
}

void findVariablesInMethod(std::string textMethod, std::vector<VariableInfo> &globalVariables, std::vector<VariableInfo> &localVariables, std::string methodName)
{
	bool isGlobalUsed = false;
	std::vector<std::string> methodLines;
	std::vector<VariableInfo> methodVariables;
	
	//split method text to lines
	int i = 0;
	while (i < textMethod.length())
	{
		std::string line = "";
		while (textMethod[i] != '\n')
		{
			line += textMethod[i];
			++i;
		}
		methodLines.push_back(line);
		++i;
	}

	//search virables in line
	std::vector<std::string>::iterator iterator = methodLines.begin();
	while (iterator != methodLines.end())
	{
		findVariableInLine(*iterator, methodVariables, methodName);
		++iterator;
	}

	//calculate first member of difficulty
	std::vector<VariableInfo>::iterator methodVarIterator = methodVariables.begin();
	std::vector<VariableInfo>::iterator globalVarIterator = globalVariables.begin();
	while (globalVarIterator != globalVariables.end())
	{
		methodVarIterator = methodVariables.begin();
		while (methodVarIterator != methodVariables.end())
		{
			if ((*globalVarIterator).variableName != (*methodVarIterator).variableName)
			{
				++(*globalVarIterator).difficult;
				break;
			}
			++methodVarIterator;
		}
		++globalVarIterator;
	}

	//separate local variables
	methodVarIterator = methodVariables.begin();
	while (methodVarIterator != methodVariables.end())
	{
		std::vector<VariableInfo>::iterator globalVarIterator = globalVariables.begin();
		while (globalVarIterator != globalVariables.end())
		{
			if ((*methodVarIterator).variableName == (*globalVarIterator).variableName)
			{
				globalVariables.erase(globalVarIterator);
				localVariables.push_back(*methodVarIterator);
				break;
			}
			else
			{
				++globalVarIterator;
			}
		}
		++methodVarIterator;
	}

}

std::vector<std::string> findMethodsCalls(std::string textMethod, std::vector<std::string> methodNames)
{
	std::vector<std::string> result;
	std::vector<std::string> methodLines;
	
	//split text in lines
	int i = 0;
	while (i < textMethod.length())
	{
		std::string line = "";
		while (textMethod[i] != '\n')
		{
			line += textMethod[i];
			++i;
		}
		methodLines.push_back(line);
		++i;
	}

	//search for calls in line
	std::vector<std::string>::iterator linesIterator = methodLines.begin();
	while (linesIterator != methodLines.end())
	{
		std::string res = findCallInMethod(*linesIterator, methodNames);
		if (!res.empty()) 
			result.push_back(res);
		++linesIterator;
	}

	return result;
}

std::string findCallInMethod(std::string line, std::vector<std::string> methodNames)
{
	std::string result = "";

	std::smatch match;
	std::regex regularMethodCall("(\\w+)\\((.*)\\);");

	if (std::regex_search(line, match, regularMethodCall))
	{
		std::vector<std::string>::iterator methodNameIterator = methodNames.begin();
		while (methodNameIterator != methodNames.end())
		{
			if (match[1] == *methodNameIterator)
				result = match[2];
			++methodNameIterator;
		}
	}

	return result;
}

void fillMethodNameStruct(std::vector<std::string> &methodNames)
{
	std::vector<std::string>::iterator methodsNameIterator = methodNames.begin();
	while (methodsNameIterator != methodNames.end())
	{
		MethodCallsInfo tmp;
		tmp.called = 0;
		tmp.calls = 0;
		tmp.methodName = *methodsNameIterator;
		MethodsCalls.push_back(tmp);
		++methodsNameIterator;
	}
}


void calculateVariablesDifficulty(std::vector<std::string> methodNames, std::vector<VariableInfo> &globalVariables, std::vector<VariableInfo> &localVariables, int numberOfMethods)
{
	std::vector<std::string>::iterator methodTextIterator = MethodsText.begin();
	std::vector<std::string>::iterator methodNameIterator = methodNames.begin();
	while (methodTextIterator != MethodsText.end())
	{
		std::vector<std::string> parametrListInMethod = findMethodsCalls(*methodTextIterator, methodNames);
		std::vector<std::string>::iterator paramIterator = parametrListInMethod.begin();
		while (paramIterator != parametrListInMethod.end())
		{
			std::vector<VariableInfo>::iterator localVariablesIterator = localVariables.begin();
			while (localVariablesIterator != localVariables.end())
			{
				if (((*localVariablesIterator).methodsName == *methodNameIterator) &&
					((*paramIterator).find((*localVariablesIterator).variableName) != (size_t)(-1)))
				{
					++(*localVariablesIterator).difficult;
				}
				++localVariablesIterator;
			}

			std::vector<VariableInfo>::iterator globalVariablesIterator = globalVariables.begin();
			while (globalVariablesIterator != globalVariables.end())
			{
				if ((*paramIterator).find((*globalVariablesIterator).variableName) != (size_t)(-1))
				{
					++(*globalVariablesIterator).numberOfCalls;
				}
				++globalVariablesIterator;
			}
			++paramIterator;

		}

		++methodTextIterator;
		++methodNameIterator;
	}

	std::vector<VariableInfo>::iterator globalVariablesIterator = globalVariables.begin();
	globalVariablesIterator = globalVariables.begin();
	while (globalVariablesIterator != globalVariables.end())
	{
		(*globalVariablesIterator).difficult /= numberOfMethods;
		(*globalVariablesIterator).difficult *= (*globalVariablesIterator).numberOfCalls;
		++globalVariablesIterator;
	}
	std::vector<VariableInfo>::iterator localVariablesIterator = localVariables.begin();
	while (localVariablesIterator != localVariables.end())
	{
		(*localVariablesIterator).difficult /= numberOfMethods;
		++localVariablesIterator;
	}
}

int calculateMethodCalls(std::vector<std::string> methodNames)
{
	std::vector<std::string>::iterator methodTextIterator = MethodsText.begin();
	std::vector<std::string>::iterator methodCurrentNameIterator = methodNames.begin();
	while (methodCurrentNameIterator != methodNames.end())
	{
		std::vector<std::string>::iterator methodNameIterator = methodNames.begin();
		while (methodNameIterator != methodNames.end())
		{
			std::vector<MethodCallsInfo>::iterator methodCallsIterator = MethodsCalls.begin();
			if ((*methodTextIterator).find(*methodNameIterator) != (size_t)(-1))
			{
				methodCallsIterator = MethodsCalls.begin();
				while (methodCallsIterator != MethodsCalls.end())
				{
					if ((*methodCallsIterator).methodName == *methodNameIterator)
						++(*methodCallsIterator).called;
					if ((*methodCallsIterator).methodName == *methodCurrentNameIterator)
						++(*methodCallsIterator).calls;
					++methodCallsIterator;
				}
			}
			++methodNameIterator;
		}
		++methodTextIterator;
		++methodCurrentNameIterator;
	}

	int result = 0;
	std::vector<MethodCallsInfo>::iterator methodCallsIterator = MethodsCalls.begin();
	while (methodCallsIterator != MethodsCalls.end())
	{
		result += ((*methodCallsIterator).calls + (*methodCallsIterator).called);
		std::cout << (*methodCallsIterator).methodName << " " << (*methodCallsIterator).calls << " " << (*methodCallsIterator).called << std::endl;
		++methodCallsIterator;
	}
	return result;
}

void calculateSpenLocal(std::vector<std::string> methodNames ,std::vector<VariableInfo> &localVariables)
{
	std::vector<VariableInfo>::iterator localVariablesIterator = localVariables.begin();
	while (localVariablesIterator != localVariables.end())
	{
		std::vector<std::string>::iterator methodNamesIterator = methodNames.begin();
		std::vector<std::string>::iterator methodTextIterator = MethodsText.begin();
		while (methodNamesIterator != methodNames.end())
		{
			if ((*localVariablesIterator).methodsName == *methodNamesIterator)
				break;
			++methodTextIterator;
			++methodNamesIterator;
		}

		std::string temp = *methodTextIterator;
		std::string variableName = (*localVariablesIterator).variableName;
		int variablePosition = 0;
		while ((variablePosition = temp.find(variableName)) > 0 )
		{
			if (!isalpha(temp[variablePosition-1]) && !isalpha(temp[variablePosition+variableName.length()]))
				++(*localVariablesIterator).spen;
			temp = temp.substr(variablePosition + variableName.length(), temp.length());
			
		}
		--(*localVariablesIterator).spen;
		++localVariablesIterator;
	}
}

void calculateSpenGlobal(std::vector<VariableInfo> &globalVariables)
{
	std::string fullText;
	std::vector<std::string>::iterator fileLinesIterator = FileLines.begin();
	while (fileLinesIterator != FileLines.end())
	{
		fullText += *fileLinesIterator;
		++fileLinesIterator;
	}

	std::vector<VariableInfo>::iterator globalVariablesIterator = globalVariables.begin();
	while (globalVariablesIterator != globalVariables.end())
	{
		std::string temp = fullText;
		std::string variableName = (*globalVariablesIterator).variableName;
		int variablePosition = 0;
		while ((variablePosition = temp.find(variableName)) > 0 )
		{
			if (!isalpha(temp[variablePosition-1]) && !isalpha(temp[variablePosition+variableName.length()]))
				++(*globalVariablesIterator).spen;
			temp = temp.substr(variablePosition + variableName.length(), temp.length());
			
		}
		--(*globalVariablesIterator).spen;
		++globalVariablesIterator;
	}
}