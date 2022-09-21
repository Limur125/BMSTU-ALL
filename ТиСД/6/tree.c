#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#include "tree.h"

int height(tree_t *p)
{
	return p?p->height:0;
}

void fixheight(tree_t* p)
{
	int hl = height(p->left);
	int hr = height(p->right);
	p->height = (hl>hr?hl:hr)+1;
}

tree_t *add(tree_t *root, tree_t *leaf)
{
    int cmp;

    if (root == NULL)
        return leaf;
    
    
    cmp = strcmp(leaf->s, root->s);

    if (cmp < 0)
    {
        root->left = add(root->left, leaf);
    }
    else
    {
        root->right = add(root->right, leaf);
    }
    fixheight(root);
    return root;

}

int bfactor(tree_t* p)
{
	return height(p->right)-height(p->left);
}

tree_t *rotateright(tree_t *p)
{
	tree_t *q = p->left;
	p->left = q->right;
	q->right = p;
	fixheight(p);
	fixheight(q);
	return q;
}

tree_t *rotateleft(tree_t *q)
{
	tree_t *p = q->right;
	q->right = p->left;
	p->left = q;
	fixheight(q);
	fixheight(p);
	return p;
}

tree_t *balance(tree_t *p)
{
	fixheight(p);
	if(bfactor(p) >= 2)
	{
		if(bfactor(p->right) < 0)
			p->right = rotateright(p->right);
		return rotateleft(p);
	}
	if(bfactor(p) <= -2)
	{
		if(bfactor(p->left) > 0)
			p->left = rotateleft(p->left);
		return rotateright(p);
	}
	return p;
}

tree_t *create_node(char s[])
{
    tree_t *tmp = malloc(sizeof(*tmp));
    if (tmp != NULL)
    {
        strcpy(tmp->s, s);
        tmp->left = NULL;
        tmp->right = NULL;
        tmp->height = 1;
    }
    return tmp;
}

tree_t *add_bal(tree_t *root, tree_t *leaf)
{
    int cmp;

    if (root == NULL)
        return leaf;

    cmp = strcmp(leaf->s, root->s);

    if (cmp < 0)
        root->left = add_bal(root->left, leaf);
    else
        root->right = add_bal(root->right, leaf);

    return balance(root);
}

void print_tree(tree_t *root, int p)
{
    int i;
    
    if (root != NULL)
    {
        print_tree(root->right, p + 4);
        for (i = 0; i < p; i++)
            printf(" ");
        printf("%-s\n", root->s);
        print_tree(root->left, p + 4);
    }
}
tree_t *search(tree_t *root, char s[], int *cmps)
{
    *cmps = 0;
    while (root != NULL)
    {
        (*cmps)++;
        if (strcmp(root->s, s) < 0)
            root = root->right;
        else if (strcmp(root->s, s) > 0)
            root = root->left;
        else if (strcmp(root->s, s) == 0)
            return root;
    }
    return root;
}

tree_t *copy_tree(tree_t *root)
{
    tree_t *tmp = NULL;
    if (root != NULL)
    {
        tmp = create_node(root->s);
        tmp->height = root->height;
        tmp->left = copy_tree(root->left);
        tmp->right = copy_tree(root->right);
    }
    return tmp;
}

void node_to_dot(tree_t *tree, FILE *f)
{
    if (tree == NULL)
        return;

    if (tree->left)
        fprintf(f, "%s_%d -> %s_%d;\n", tree->s, tree->height, tree->left->s, tree->left->height);

    if (tree->right)
        fprintf(f, "%s_%d -> %s_%d;\n", tree->s, tree->height, tree->right->s, tree->right->height);
    node_to_dot(tree->left, f);
    node_to_dot(tree->right, f);
}

void tree_export_to_dot(FILE *f, const char *tree_name, tree_t *tree)
{
    fprintf(f, "digraph %s {\n", tree_name);
    node_to_dot(tree, f);
    fprintf(f, "}\n");
}