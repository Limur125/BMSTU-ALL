#define _CRT_SECURE_NO_WARNINGS
#include <cstdio>
#include <fstream>
#include <iostream>
#include <map>
#include <unordered_map>
#include <vector>
#include <algorithm>
#include <chrono>
#define ll unsigned long long int

namespace Huffman {

    std::string HuffmanValue[256] = { "" };

    struct Node {
        char character;
        ll count;
        Node* left, * right;

        Node(ll count) {
            this->character = 0;
            this->count = count;
            this->left = this->right = nullptr;
        }

        Node(char character, ll count) {
            this->character = character;
            this->count = count;
            this->left = this->right = nullptr;
        }
    };

    namespace Utility {
        ll get_file_size(const char* filename) {
            FILE* p_file = fopen(filename, "rb");
            fseek(p_file, 0, SEEK_END);
            ll size = ftello64(p_file);
            fclose(p_file);
            return size;
        }

        void Inorder(Node* root, std::string& value) {
            if (root) {
                value.push_back('0');
                Inorder(root->left, value);
                value.pop_back();

                if (root->left == nullptr && root->right == nullptr) {
                    printf("Character: %c, Count: %lld, ", root->character, root->count);
                    std::cout << "Huffman Value: " << value << std::endl;
                }

                value.push_back('1');
                Inorder(root->right, value);
                value.pop_back();
            }
        }
    };

    namespace CompressUtility {

        Node* combine(Node* a, Node* b) {
            Node* parent = new Node((a ? a->count : 0) + (b ? b->count : 0));
            parent->left = b;
            parent->right = a;
            return parent;
        }

        bool sortbysec(const Node* a, const Node* b) {
            return (a->count > b->count);
        }

        std::map<char, ll>parse_file(const char* filename, const ll Filesize) {
            FILE* ptr = fopen(filename, "rb");
            if (ptr == NULL) {
                perror("Error: File not found:");
                exit(-1);
            }
            unsigned char ch;
            ll size = 0, filesize = Filesize;
            std::vector<ll>Store(256, 0);

            while (size != filesize) {
                ch = fgetc(ptr);
                ++Store[ch];
                ++size;
            }

            std::map<char, ll>store;
            for (int i = 0; i < 256; ++i) {
                if (Store[i]) {
                    store[i] = Store[i];
                }
            }
            fclose(ptr);
            return store;
        }

        std::vector<Node*>sort_by_character_count(const std::map<char, ll>& value) {
            std::vector<Node*> store;
            for (auto& x : value) {
                store.push_back(new Node(x.first, x.second));
            }
            sort(store.begin(), store.end(), sortbysec);
            return store;
        }

        std::string generate_header(const char padding) {
            std::string header = "";
            unsigned char UniqueCharacter = 255;

            for (int i = 0; i < 256; ++i) {
                if (HuffmanValue[i].size()) {
                    header.push_back(i);
                    header.push_back(HuffmanValue[i].size());
                    header += HuffmanValue[i];
                    ++UniqueCharacter;
                }
            }
            char value = UniqueCharacter;
            return value + header + (char)padding;
        }

        ll store_huffman_value(const Node* root, std::string& value) {
            ll temp = 0;
            if (root) {
                value.push_back('0');
                temp = store_huffman_value(root->left, value);
                value.pop_back();
                if (!root->left && !root->right) {
                    HuffmanValue[(unsigned char)root->character] = value;
                    temp += value.size() * root->count;
                }
                value.push_back('1');
                temp += store_huffman_value(root->right, value);
                value.pop_back();
            }
            return temp;
        }

        Node* generate_huffman_tree(const std::map <char, ll>& value) {
            std::vector<Node*> store = sort_by_character_count(value);
            Node* one, * two, * parent;
            sort(begin(store), end(store), sortbysec);
            if (store.size() == 1) {
                return combine(store.back(), nullptr);
            }
            while (store.size() > 2) {
                one = *(store.end() - 1); two = *(store.end() - 2);
                parent = combine(one, two);
                store.pop_back(); store.pop_back();
                store.push_back(parent);

                std::vector<Node*>::iterator it1 = store.end() - 2;
                while ((*it1)->count < parent->count && it1 != begin(store)) {
                    --it1;
                }
                std::sort(it1, store.end(), sortbysec);
            }
            one = *(store.end() - 1); two = *(store.end() - 2);
            return combine(one, two);
        }

        void compress(const char* filename, const ll Filesize, const ll PredictedFileSize) {
            const char padding = (8 - ((PredictedFileSize) & (7))) & (7);
            const std::string header = generate_header(padding);
            int header_i = 0;
            const int h_length = header.size();
            std::cout << "Padding size: " << (int)padding << std::endl;
            FILE* iptr = fopen(filename, "rb"), * optr = fopen((std::string(filename) + ".abiz").c_str(), "wb");

            if (!iptr) {
                perror("Error: File not found: ");
                exit(-1);
            }

            while (header_i < h_length) {
                fputc(header[header_i], optr);
                ++header_i;
            }

            unsigned char ch, fch = 0;
            char counter = 7;
            ll size = 0, i;
            while (size != Filesize) {
                ch = fgetc(iptr);
                i = 0;
                const std::string& huffmanStr = HuffmanValue[ch];
                while (huffmanStr[i] != '\0') {
                    fch = fch | ((huffmanStr[i] - '0') << counter);
                    counter = (counter + 7) & 7;
                    if (counter == 7) {
                        fputc(fch, optr);
                        fch ^= fch;
                    }
                    ++i;
                }
                ++size;
                if ((size * 100 / Filesize) > ((size - 1) * 100 / Filesize)) {
                    printf("\r%lld%% completed  ", (size * 100 / Filesize));
                }
            }
            if (fch) {
                fputc(fch, optr);
            }
            printf("\n");
            fclose(iptr);
            fclose(optr);
        }

    };

    namespace DecompressUtility {
        void generate_huffman_tree(Node* const root, const std::string& codes, const unsigned char ch) {
            Node* traverse = root;
            int i = 0;
            while (codes[i] != '\0') {
                if (codes[i] == '0') {
                    if (!traverse->left) {
                        traverse->left = new Node(0);
                    }
                    traverse = traverse->left;
                }
                else {
                    if (!traverse->right) {
                        traverse->right = new Node(0);
                    }
                    traverse = traverse->right;
                }
                ++i;
            }
            traverse->character = ch;
        }

        std::pair<Node*, std::pair<unsigned char, int>>decode_header(FILE* iptr) {
            Node* root = new Node(0);
            int charactercount, buffer, total_length = 1;
            char ch, len;
            charactercount = fgetc(iptr);
            std::string codes;
            ++charactercount;
            while (charactercount) {
                ch = fgetc(iptr);
                codes = "";
                len = fgetc(iptr);
                buffer = len;

                while (buffer > codes.size()) {
                    codes.push_back(fgetc(iptr));
                }
                total_length += codes.size() + 2;

                generate_huffman_tree(root, codes, ch);
                --charactercount;
            }
            unsigned char padding = fgetc(iptr);
            ++total_length;
            return { root, {padding, total_length} };
        }
 
        void decompress(const char* filename, const ll Filesize, const ll leftover) {
            const std::string fl = filename;
            FILE* iptr = fopen(fl.c_str(), "rb");
            FILE* optr = fopen(std::string("output" + fl.substr(0, fl.size() - 5)).c_str(), "wb");

            if (iptr == NULL) {
                perror("Error: File not found");
                exit(-1);
            }

            std::pair<Node*, std::pair<unsigned char, int>>HeaderMetadata = decode_header(iptr);
            Node* const root = HeaderMetadata.first;
            const auto [padding, headersize] = HeaderMetadata.second;

            char ch, counter = 7;
            ll size = 0;
            const ll filesize = Filesize - headersize;
            Node* traverse = root;
            ch = fgetc(iptr);
            while (size != filesize) {
                while (counter >= 0) {
                    traverse = ch & (1 << counter) ? traverse->right : traverse->left;
                    ch ^= (1 << counter);
                    --counter;
                    if (!traverse->left && !traverse->right) {
                        fputc(traverse->character, optr);
                        if (size == filesize - 1 && padding == counter + 1) {
                            break;
                        }
                        traverse = root;
                    }
                }
                ++size;
                counter = 7;
                if ((size * 100 / filesize) > ((size - 1) * 100 / filesize)) {
                    printf("\r%lld%% completed, size: %lld bytes   ", (size * 100 / filesize), size);
                }
                ch = fgetc(iptr);
            }
            fclose(iptr);
            fclose(optr);
        }

    }

};

using namespace Huffman;

int main(int argc, char* argv[]) {
    if (argc != 3) {
        printf("Usage:\n %s (-c FileToBeCompressed | -dc FileToBeDecompressed)\n", argv[0]);
        exit(-1);
    }
    const char* option = argv[1], * filename = argv[2];
    printf("%s\n", filename);

    std::chrono::time_point <std::chrono::system_clock> start, end;
    std::chrono::duration <double> time;
    ll filesize, predfilesize;
    if (std::string(option) == "-c") {
        filesize = Utility::get_file_size(filename);
        auto mapper = CompressUtility::parse_file(filename, filesize);
        Node* const root = CompressUtility::generate_huffman_tree(mapper);
        std::string buf = "";
        predfilesize = CompressUtility::store_huffman_value(root, buf);
        printf("Original File: %lld bytes\n", filesize);
        printf("Compressed File Size (without header): %lld bytes\n", (predfilesize + 7) >> 3);

        start = std::chrono::system_clock::now();
        CompressUtility::compress(filename, filesize, predfilesize);
        end = std::chrono::system_clock::now();

        time = (end - start);
        std::cout << "Compression Time: " << time.count() << "s" << std::endl;

    }
    else if (std::string(option) == "-dc") {
        filesize = Utility::get_file_size(filename);
        start = std::chrono::system_clock::now();
        DecompressUtility::decompress(filename, filesize, predfilesize);
        end = std::chrono::system_clock::now();

        time = (end - start);
        std::cout << "\nDecompression Time: " << time.count() << "s" << std::endl;
    }
    else {
        std::cout << "\nInvalid Option... Exiting\n";
    }
    return 0;
}