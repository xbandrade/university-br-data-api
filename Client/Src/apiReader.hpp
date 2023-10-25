#ifndef APIREADER_HPP
#define APIREADER_HPP

#include <iostream>
#include <map>
#include <unordered_set>
#include <algorithm>
#include <curl/curl.h>
#include <nlohmann/json.hpp>

using json = nlohmann::json;

size_t writeCallback(void* contents, size_t size, size_t nmemb, void* userp);

class ApiReader {
public:
	ApiReader();
	std::map<int, std::string> getApiResponse(const std::string& apiUrl);
	int getTotalItems();
	int getCurrentPage();
	int getTotalPages();

private:
	int totalItems;
	int currentPage;
	int totalPages;
	std::string nextPage;
	std::string prevPage;
};

#endif  // APIREADER_HPP