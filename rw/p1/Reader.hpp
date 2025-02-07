#pragma once
#include <string>
#include <iostream>
#include <fstream>


class Reader
{
public:

	double** kMatr;
	double* TArr, * nuArr, * tzArr;
	double* zArr;
	int TN, nuN, zN;

	Reader(std::string filename) : filename(filename) { }
	
	bool Read()
	{
		std::ifstream in(filename);
		if (!in.is_open())
		{
			return false;
		}
		in >> nuN;
		nuArr = new double[nuN];
		for (int i = 0; i < nuN; i++)
		{
			in >> nuArr[i];
		}
		in >> TN;
		TArr = new double[TN];
		kMatr = new double* [TN];
		for (int i = 0; i < TN; i++)
		{
			in >> TArr[i];
			kMatr[i] = new double[nuN - 1];
		}
		
		for (int i = 0; i < TN; i++)
		{
			for (int j = 0; j < nuN - 1; j++)
			{
				in >> kMatr[i][j];
			}
		}
		in.close();
		return true;
	}

	bool ReadTemp(std::string filename)
	{
		std::ifstream in(filename);
		if (!in.is_open())
		{
			return false;
		}
		in >> zN;
		
		zArr = new double[zN];
		tzArr = new double[zN];
		for (int i = 0; i < zN; i++)
		{
			in >> zArr[i] >> tzArr[i];
		}
		
		in.close();
		return true;
	}

	~Reader() 
	{
		delete TArr;
		delete nuArr;
		for (int i = 0; i < TN; i++)
		{
			delete kMatr[i];
		}
		delete kMatr;
	}

private:
	std::string filename;
};
