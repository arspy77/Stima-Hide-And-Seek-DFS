#include <bits/stdc++.h>
using namespace std;

bool added[10000000];

int main() {
	int x;
	srand(time(nullptr));
	cin >> x;
	cout << x << '\n';
	added[0] = true;
	for (int i = 1; i < x; ++i) {
		int a = rand()%i;
		int b = rand()%(x-i);
		int p = 0, q = 0;
		while (a >= 0) {
			if (added[p]) --a;
			if (a >= 0) ++p;
		}
		while (b >= 0) {
			if (!added[q]) --b;
			if (b >= 0) ++q;
		}
		cout << p+1 << ' ' << q+1 << '\n';
		added[q] = true;
	}
}