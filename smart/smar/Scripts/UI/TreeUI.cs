using Godot;
using System;
using System.Collections.Generic;
using BinaryTree;

public partial class TreeUI : Control
{
    private object _arbol;
    private float _radioNodo = 20f;
    private float _espaciadoVertical = 80f;
    private float _espaciadoHorizontal = 60f;

    public void SetArbol(object arbol)
    {
        _arbol = arbol;
        GD.Print("🎨 SetArbol llamado con tipo: ", arbol?.GetType().Name);
        CallDeferred(nameof(QueueRedraw));
    }

    public override void _Draw()
    {
        if (_arbol == null)
        {
            GD.Print("⚠️ _arbol es null, no se dibuja nada");
            return;
        }

        GD.Print("🖌️ _Draw llamado con tipo de árbol: ", _arbol.GetType().Name);

        string tipoTexto = "";

        if (_arbol is BST bst)
        {
            var root = bst.GetRoot();
            GD.Print("🌿 BST root: ", root?.Value);
            tipoTexto = "BST";
            if (root != null)
                DibujarNodoBST(root, Size.X / 2, 40, Size.X / 4);
        }
        else if (_arbol is AVLTree avl)
        {
            var root = avl.GetRoot();
            GD.Print("🌳 AVL root: ", root == null ? "null" : root.Value.ToString());
            tipoTexto = "AVL";
            if (root != null)
                DibujarNodoAVL(root, Size.X / 2, 40, Size.X / 4);
            else
                GD.Print("❌ AVL root es null, no se dibuja");
        }
        else
        {
            GD.Print("❓ Tipo de árbol no reconocido: ", _arbol.GetType().Name);
        }

        if (!string.IsNullOrEmpty(tipoTexto))
        {
            var font = GetThemeDefaultFont();
            DrawString(font, new Vector2(10, 20), $"Árbol: {tipoTexto}", modulate: Colors.White);
        }
    }

    private void DibujarNodoBST(BST.Node nodo, float x, float y, float offsetX)
    {
        GD.Print("🟢 Dibujando nodo BST con valor: ", nodo.Value);

        if (nodo.Left != null)
        {
            float childX = x - offsetX;
            float childY = y + _espaciadoVertical;
            DrawLine(new Vector2(x, y), new Vector2(childX, childY), Colors.Black, 2);
            DibujarNodoBST(nodo.Left, childX, childY, offsetX * 0.5f);
        }

        if (nodo.Right != null)
        {
            float childX = x + offsetX;
            float childY = y + _espaciadoVertical;
            DrawLine(new Vector2(x, y), new Vector2(childX, childY), Colors.Black, 2);
            DibujarNodoBST(nodo.Right, childX, childY, offsetX * 0.5f);
        }

        DrawCircle(new Vector2(x, y), _radioNodo, new Color(0.6f, 1f, 0.6f));
        var texto = nodo.Value.ToString();
        var font = GetThemeDefaultFont();
        var textSize = font.GetStringSize(texto);
        DrawString(font, new Vector2(x - textSize.X / 2, y + textSize.Y / 2), texto, modulate: Colors.Black);
    }

    private void DibujarNodoAVL(AVLTree.Node nodo, float x, float y, float offsetX)
    {
        GD.Print("🔵 Dibujando nodo AVL con valor: ", nodo.Value);

        if (nodo.Left != null)
        {
            float childX = x - offsetX;
            float childY = y + _espaciadoVertical;
            DrawLine(new Vector2(x, y), new Vector2(childX, childY), Colors.Black, 2);
            DibujarNodoAVL(nodo.Left, childX, childY, offsetX * 0.5f);
        }

        if (nodo.Right != null)
        {
            float childX = x + offsetX;
            float childY = y + _espaciadoVertical;
            DrawLine(new Vector2(x, y), new Vector2(childX, childY), Colors.Black, 2);
            DibujarNodoAVL(nodo.Right, childX, childY, offsetX * 0.5f);
        }

        DrawCircle(new Vector2(x, y), _radioNodo, new Color(0.4f, 0.6f, 1f));
        var texto = nodo.Value.ToString();
        var font = GetThemeDefaultFont();
        var textSize = font.GetStringSize(texto);
        DrawString(font, new Vector2(x - textSize.X / 2, y + textSize.Y / 2), texto, modulate: Colors.Black);
    }

    private Font GetThemeDefaultFont()
    {
        return GetThemeFont("font", "Label");
    }
}
