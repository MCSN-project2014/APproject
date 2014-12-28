﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APproject
{
    public class ASTGenerator
    {
        private Node AST;
        private Node root;

        public ASTGenerator()
        {

	    }

        public void initAST(Node node) 
        {
            AST = node;
            root = AST;
        }

        public void addChildren(Node node) 
        {
            AST.addChildren(node);
        }

        public ASTNode getRoot()
        {
            return root;
        }
    }
}
