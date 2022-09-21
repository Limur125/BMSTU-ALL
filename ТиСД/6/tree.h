#ifndef _TREE_H_
#define _TREE_H_

#define STR_LEN 50

typedef struct tree
{
    char s[STR_LEN];
    int height;
    struct tree *left;
    struct tree *right;
} tree_t;


tree_t *add(tree_t *root, tree_t *leaf);
tree_t *balance(tree_t *p);
tree_t *create_node(char s[]);
void print_tree(tree_t *root, int p);
tree_t *search(tree_t *root, char s[], int *cmps);
tree_t *copy_tree(tree_t *root);
tree_t *add_bal(tree_t *root, tree_t *leaf);
void tree_export_to_dot(FILE *f, const char *tree_name, tree_t *tree);

#endif