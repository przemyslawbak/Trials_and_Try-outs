#include <iostream>
#include <fstream>
#include <sstream>
#include <vector>
#include <array>
#include <set>
#include <map>
#include <utility>
#include <algorithm>


int main()
{
	std::vector<int> v = { 1, 2, 3, 4, 5 };

	std::array<int, 5> arr = { 1, 2, 3, 4, 5 };
	for (auto el : arr)
	{
		std::cout << el << '\n';
	}

	std::set<int> myset = { 1, 2, 3, 4, 5 };
	for (auto el : myset)
	{
		std::cout << el << '\n';
	}

	std::set<int> myset = { 1, 2, 3, 4, 5 };
	myset.insert(10);
	myset.insert(42);
	for (auto el : myset)
	{
		std::cout << el << '\n';
	}

	std::map<int, char> mymap = { {1, 'a'}, {2, 'b'}, {3,'w'} };
	for (auto el : mymap)
	{
		std::cout << el.first << ' ' << el.second << '\n';
	}

	int x = 123;
	double d = 3.14;
	std::pair<int, double> mypair = std::make_pair(x, d);
	std::cout << "The first element is: " << mypair.first << '\n';
	std::cout << "The second element is: " << mypair.second << '\n';

	v.push_back(10);
	for (int el : v)
	{
		std::cout << el << '\n';
	}

	std::sort(v.begin(), v.end());
	for (auto el : v)
	{
		std::cout << el << '\n';
	}

	auto result = std::find(v.begin(), v.end(), 5);
	if (result != v.end())
	{
		std::cout << "Element found: " << *result;
	}
	else
	{
		std::cout << "Element not found.";
	}

	auto result2 = std::find(std::begin(v), std::end(v), 5);
	if (result2 != std::end(v))
	{
		std::cout << "An element found: " << *result2;
	}
	else
	{
		std::cout << "Element not found.";
	}

	std::vector<int> copy_from_v = { 1, 2, 3, 4, 5 };
	std::vector<int> copy_to_v(5); // reserve the space for 5 elements
	std::copy(copy_from_v.begin(), copy_from_v.end(), copy_to_v.begin());
	for (auto el : copy_to_v)
	{
		std::cout << el << '\n';
	}
}