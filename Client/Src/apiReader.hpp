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
	std::map<int, std::string> getApiResponseAll(const std::string& apiUrl);
	std::map<std::string, std::string> getApiResponseByPK(const std::string& apiUrl, int pk);
	int getTotalItems();
	int getCurrentPage();
	int getTotalPages();
	std::string getNextPage();
	std::string getPrevPage();

private:
	int totalItems;
	int currentPage;
	int totalPages;
	std::string nextPage;
	std::string prevPage;
};

#endif  // APIREADER_HPP