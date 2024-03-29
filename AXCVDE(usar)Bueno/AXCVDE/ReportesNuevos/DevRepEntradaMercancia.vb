﻿Imports DevExpress.XtraReports.UI

Public Class DevRepEntradaMercancia
    Private wgReporte As Integer
    Private sParametro As String
    Private dsReporte As New DataSet

    Public Sub CargaReporte(ByVal ds2 As DataSet, ByVal reporte As Integer)
        dsReporte = ds2
        Me.DataSource = dsReporte
        DataMember = ""
    End Sub

    Public Function CreateReport() As XtraReport
        ' Create a dataset.           
        Dim ds As DataSet = dsReporte
        ' Define a report
        Dim report As New XtraReport() With {
            .DataSource = ds, .DataMember = ds.Tables(1).TableName
        }
        Return report
    End Function

    Private Sub DevRepExistencias_AfterPrint(sender As Object, e As EventArgs) Handles MyBase.AfterPrint
        Try
            Me.DataSource = Nothing
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "AXC")
        End Try
    End Sub
End Class