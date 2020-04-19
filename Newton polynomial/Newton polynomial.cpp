#include "pch.h"
#include <iostream>
#include <math.h>

using namespace std;

int main()
{
	double** y = new double*[100]; //Выделение памяти для таблицы интерполяции
	double x;
	int n;

	for (int i = 0; i < 100; i++) {
		y[i] = new double[100];
	}

	cout << "Function:" << endl;
	cout << "x\t" << "y\t" << endl;

	for (int i = 0; i < 11; i++) { //Заполнение таблицы точками, сгенерированными проихводящей функцией
		y[0][i] = i; 
		cout << y[0][i]; 

		y[1][i] = y[0][i] * y[0][i]; //Производящая функция y = x*x
		cout << '\t' << y[1][i];

		cout << '\n';
	}

	int r = 0;

	while (r < 3) { //Можно посчитать три произвольно взятые точки
		cout << "\n\nInput X:" << endl;
		cin >> x;
		
		if (x < y[0][0] || x > y[0][10]) { 
			/*Условие: если координата x интерполируемой точки
			меньше координаты x первой точки таблицы или больше координаты x
			последней точки таблицы (выход за границы массива), то ошибка*/
			cout << "\nImpossible to calculate function value! Going abroad table.";
		}
		
		/*Иначе: проводим интерполирование точки*/
		else {
			cout << "\nInput polynomial degree:" << endl; //Вводим степень полинома
			cin >> n;
			cout << endl;

			int k = 0; //Счетчик в выражении (x0 - xn) в формуле для конечной разности (знаменатель формулы)
			int t = -1; //Счетчик для обозначения x[i] в интервале интерполяции
			int q = -1; //Счетчик для обозначения y[i] в интервале интерполяции

			if ((int)x - n/2 < 0)  {
				cout << "Intepolation interval:" << endl; //Вывод интервала интерполяции
				cout << "\txi\t" << "\tyi\t" << endl;
				
				for (int i = (int)x; i <= (int)x + n; i++) {

					t = t + 1;
					y[0][i] = t;
					cout << "x" << "[" << y[0][i] << "]";

					y[1][i] = i; //Вывод координат x точек, попавших в интервал интерполяции
					cout << '\t' << y[1][i];

					q = q + 1;

					y[2][i] = q;
					cout << '\t' << "y" << "[" << y[2][i] << "]";

					y[3][i] = y[1][i] * y[1][i];
					cout << '\t' << y[3][i];

					cout << '\n';
				}
				for (int j = 4, k = 1; j <= 4 + n, k < n + 1; j++, k++) {
					for (int i = (int)x; i <= (int)x  + n; i++) {
						y[j][i] = (y[j - 1][i] - y[j - 1][i + 1]) / (y[1][i] - y[1][i + k]);
					}
				}

				cout << "\nFunction value: " << endl;

				double a = 1;
				double b = 0;
				double c = 0;
				int j = 0;
				int i = 0;

				for (i = (int)x, j = 4; i <= (int)x + n, j < 4 + n; i++, j++) {
					a = a * (x - y[1][i]);
					b = a * y[j][(int)x];
					c += b;

				}

				cout << "P(x) = " << y[3][(int)x] + c;
			}
			
			else if ((int)x + n/2 > 10) {
				cout << "Intepolation interval:" << endl;
				cout << "\txi\t" << "\tyi\t" << endl;

				int t = -1;
				int q = -1;

				for (int i = (int)x - n; i <= (int)x; i++) {
					t = t + 1;
					y[0][i] = t;
					cout << "x" << "[" << y[0][i] << "]";

					y[1][i] = i;
					cout << '\t' << y[1][i];

					q = q + 1;

					y[2][i] = q;
					cout << '\t' << "y" << "[" << y[2][i] << "]";

					y[3][i] = y[1][i] * y[1][i];
					cout << '\t' << y[3][i];

					cout << '\n';
				}

				for (int j = 4, k = 1; j <= 4 + n, k < n + 1; j++, k++) {
					for (int i = (int)x - n; i <= (int)x; i++) {
						y[j][i] = (y[j - 1][i] - y[j - 1][i + 1]) / (y[1][i] - y[1][i + k]);
					}
				}

				cout << "\nFunction value: " << endl;

				double a = 1;
				double b = 0;
				double c = 0;
				int j = 0;
				int i = 0;

				for (i = (int)x-n, j = 4; i <= (int)x, j < 4 + n; i++, j++) {
					a = a * (x - y[1][i]);
					b = a * y[j][(int)x-n];
					c += b;

				}

				cout << "P(x) = " << y[3][(int)x - n] + c;
			}

			else {
				cout << "Intepolation interval:" << endl;
				cout << "\txi\t" << "\tyi\t" << endl;
				
				int t = -1;
				int q = -1;

				for (int i = (int)x - n / 2; i <= ((int)x - n / 2) + n; i++) {
					t = t + 1;
					y[0][i] = t;
					cout << "x"<<"["<< y[0][i] <<"]";

					y[1][i] = i;
					cout <<'\t' <<y[1][i];

					q = q + 1;
					
					y[2][i] = q;
					cout <<'\t'<< "y" << "[" << y[2][i] << "]";
					
					y[3][i] = y[1][i] * y[1][i];
					cout << '\t' << y[3][i];

					cout << '\n';
				}
				for (int j = 4, k = 1; j <= 4 + n, k < n + 1; j++, k++) {
					for (int i = (int)x - n / 2; i <= ((int)x - n / 2) + n; i++) {
						y[j][i] = (y[j - 1][i] - y[j - 1][i + 1]) / (y[1][i] - y[1][i + k]);
					}
				}

				cout << "\nFunction value: " << endl;

				double a = 1;
				double b = 0;
				double c = 0;
				int j = 0;
				int i = 0;

				for (i = (int)x - n / 2, j = 4; i <= ((int)x - n / 2) + n, j < 4 + n; i++, j++) {
					a = a * (x - y[1][i]);
					b = a * y[j][(int)x - n / 2];
					c += b;

				}

				cout << "P(x) = " << y[3][(int)x - n / 2] + c;
			}
		}
		r++;
	}
	
	double** z = new double* [100];
	double p = 0;
	int m;
	
	for (int u = 0; u < 100; u++) {
		z[u] = new double[100];
	}

	r = 0;

	double g = -0.5;

	while (r < 3) {
		cout << "\n\nInput polynomial degree:" << endl;
		cin >> m;

		for (int u = -1; u < 20; u++) {
			g = g + 0.5;
			z[1][u] = g;
			z[0][u] = cos(z[1][u]) - z[1][u];
		}

		int l = 0;

		for (int w = 2, l = 1; w <= 2 + m, l < m + 1; w++, l++) {
			for (int u = (int)p; u <= (int)p + m; u++) {
				z[w][u] = (z[w - 1][u] - z[w - 1][u + 1]) / (z[0][u] - z[0][u + l]);
			}
		}

		cout << "\nFunction y = cos(x) - x root: \n" << endl;

		double d = 1;
		double e = 0;
		double f = 0;
		int w = 0;
		int u = 0;

		for (u = (int)p, w = 2; u <= (int)p + m, w < 2 + m; u++, w++) {
			d = d * (p - z[0][u]);
			e = d * z[w][(int)p];
			f += e;

		}

		cout << "x = " << z[1][(int)p] + f;

			r++;
	}
}
	

