#include "apiReader.hpp"

size_t writeCallback(void* contents, size_t size, size_t nmemb, void* userp) {
    size_t totalSize = size * nmemb;
    std::string* response = static_cast<std::string*>(userp);
    response->append(static_cast<char*>(contents), totalSize);
    return totalSize;
}

ApiReader::ApiReader() {
    totalItems = 0;
    currentPage = 0;
    totalPages = 0;
    nextPage = "";
    prevPage = "";
}

int ApiReader::getTotalItems() {
    return totalItems;
}

int ApiReader::getCurrentPage() {
    return currentPage;
}

int ApiReader::getTotalPages() {
    return totalPages;
}

std::map<int, std::string> ApiReader::getApiResponseAll(const std::string& apiUrl) {
    std::map<int, std::string> dataMap;
    CURL* curl = curl_easy_init();
    std::string response;
    if (curl) {
        curl_easy_setopt(curl, CURLOPT_URL, apiUrl.c_str());
        curl_easy_setopt(curl, CURLOPT_FOLLOWLOCATION, 1L);
        curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, writeCallback);
        curl_easy_setopt(curl, CURLOPT_WRITEDATA, &response);
        CURLcode res = curl_easy_perform(curl);
        if (res != CURLE_OK) {
            std::cerr << "Curl failed: " << curl_easy_strerror(res) << std::endl;
        }
        curl_easy_cleanup(curl);
    }
    if (!response.empty()) {
        try {
            json j = json::parse(response);
            if (j.contains("data") && j["data"].is_array()) {
                totalPages = j["totalPages"];
                totalItems = j["totalItems"];
                currentPage = j["currentPage"];
                
                if (!j["nextPage"].is_null()) {
                    nextPage = j["nextPage"];
                }
                else {
                    nextPage = "";
                }
                if (!j["previousPage"].is_null()) {
                    prevPage = j["previousPage"];
                }
                else {
                    prevPage = "";
                }
                for (const auto& item : j["data"]) {
                    if (item.contains("id") && item.contains("name")) {
                        int id = item["id"];
                        std::string name = item["name"];
                        std::string state = item["state"];
                        dataMap[id] = name + " - " + state;
                    }
                }
            }
        }
        catch (const std::exception& e) {
            std::cerr << "Error parsing JSON response: " << e.what() << std::endl;
        }
    }
    return dataMap;
}


std::map<std::string, std::string> ApiReader::getApiResponseByPK(const std::string& apiUrl, int pk) {
    std::map<std::string, std::string> dataMap;
    std::string fullUrl;
    size_t pos = apiUrl.find("search");
    if (pos != std::string::npos) {
        std::string baseSearchUrl = apiUrl.substr(0, pos + 6);
        fullUrl = baseSearchUrl + "/" + std::to_string(pk);
    }
    else {
        return dataMap;
    }
    CURL* curl = curl_easy_init();
    std::string response;
    if (curl) {
        curl_easy_setopt(curl, CURLOPT_URL, fullUrl.c_str());
        curl_easy_setopt(curl, CURLOPT_FOLLOWLOCATION, 1L);
        curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, writeCallback);
        curl_easy_setopt(curl, CURLOPT_WRITEDATA, &response);
        CURLcode res = curl_easy_perform(curl);
        if (res != CURLE_OK) {
            std::cerr << "Curl failed: " << curl_easy_strerror(res) << std::endl;
        }
        curl_easy_cleanup(curl);
    }
    if (!response.empty()) {
        try {
            json j = json::parse(response);
            if (j.contains("webPages") && j.contains("domains")) {
                std::string name = j["name"];
                int id = j["id"];
                std::string stdId = std::to_string(id);
                std::string webPages = j["webPages"];
                std::string domains = j["domains"];
                std::string state = j["state"];
                dataMap["id"] = stdId;
                dataMap["webPages"] = webPages;
                dataMap["domains"] = domains;
                dataMap["state"] = state;
                dataMap["name"] = name;
            }
        }
        catch (const std::exception& e) {
            std::cerr << "Error parsing JSON response: " << e.what() << std::endl;
        }
    }
    return dataMap;
}
