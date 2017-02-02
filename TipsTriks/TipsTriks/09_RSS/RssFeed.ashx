<%@ WebHandler Language="VB" Class="RssFeed" %>

Imports System
Imports System.Web

Public Class RssFeed : Inherits ProductsHttpHandlerBase
   
    Protected Overrides Sub PopulateChannel(ByVal channelName As String, ByVal userName As String)
        
        Channel.Title = "Sample Products from Northwind"
        
        Dim adapter As New NorthwindTableAdapters.ProductsTableAdapter
        Dim products As Northwind.ProductsDataTable
        Dim product As Northwind.ProductsRow
        
        products = adapter.GetProductByCategoryID(1)
        
        For Each product In products
            
            Channel.Items.Add(New ProductsItem(product.ProductID, product.ProductName, "products.aspx?productId=" & product.ProductID))
                        
        Next
                
    End Sub
    
End Class