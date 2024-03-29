﻿Imports DevExpress.XtraEditors
Imports DevExpress.XtraBars.Docking2010.Views
Imports DevExpress.XtraBars
Imports DevExpress.XtraBars.Navigation
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Columns
Public Class FrmCreaRecepcion

#Region "VARIABLES"
    Public sOrden As String = ""

#End Region

#Region "EVENTOS"


    Private Sub FrmCreaRecepcion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            LimpiarCampos()
            ListaClientes("@")

        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, "AXC", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    Private Sub txtOrdenCompra_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtOrdenCompra.KeyPress
        Try
            If e.KeyChar = Convert.ToChar(Keys.Return) Then
                If txtOrdenCompra.Text = "" Then
                    XtraMessageBox.Show("Debe capturar una orden de compra", "AXC", MessageBoxButtons.OK)
                    Return
                End If

                ConsultaOrdenCompra()
            End If
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, "AXC", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub btnBuscar_Click(sender As System.Object, e As System.EventArgs) Handles btnBuscar.Click
        Try
            If txtOrdenCompra.Text = "" Then
                XtraMessageBox.Show("Debe capturar una orden de compra", "AXC", MessageBoxButtons.OK)
                Return
            End If

            ConsultaOrdenCompra()
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, "AXC", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnNuevo_Click(sender As System.Object, e As System.EventArgs) Handles btnNuevo.Click
        Try
            LimpiarCampos()
            ListaClientes("@")
            NuevaOC()
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, "AXC", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    'Private Sub txtBusqueda_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs)
    '    Try
    '        If e.KeyChar = Convert.ToChar(Keys.Return) Then
    '            BuscaNumParte(txtBusqueda.Text)

    '        End If
    '    Catch ex As Exception
    '       XtraMessageBox.Show(ex.Message, "AXC", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    Private Sub cmbNumParte_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbNumParte.EditValueChanged
        Try
            Dim text As String = cmbNumParte.Text
            If text.Equals("System.Data.DataRowView") Then
                Return
            End If
            If cmbNumParte.Text = "TODOS" Then
                Return
            End If
            ConsultaNumParte()
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message.ToString, "AXC", MessageBoxButtons.OK)
        End Try
    End Sub

    'Private Sub cmbProveedor_SelectedIndexChanged(sender As Object, e As EventArgs)
    '    Try
    '        Dim text As String = cmbProveedor.Text
    '        If text.Equals("System.Data.DataRowView") Then
    '            Return
    '        End If
    '        ConsultaProveedor()
    '    Catch ex As Exception
    '        XtraMessageBox.Show(ex.Message.ToString, "AXC", MessageBoxButtons.OK)
    '    End Try
    'End Sub

    Private Sub btnAgregarNP_Click(sender As System.Object, e As System.EventArgs) Handles btnAgregarNP.Click
        Try
            If txtOrdenCompra.Text = "" Then
                XtraMessageBox.Show("Debe capturar una orden de compra válida", "AXC")
                Return
            End If

            If cmbCliente.Text = "" Then
                XtraMessageBox.Show("Debe capturar un cliente válido", "AXC")
                Return
            End If

            If cmbNumParte.Text = "" Then
                XtraMessageBox.Show("No hay artículo seleccionado", "AXC")
                Return
            End If

            If cmbNumParte.Text = "TODOS" Then
                XtraMessageBox.Show("No hay artículo seleccionado", "AXC")
                Return
            End If

            If txtCantidad.Text = "" Then
                XtraMessageBox.Show("No se ingreso cantidad", "AXC")
                Return
            End If

            If XtraMessageBox.Show("¿Agregar nueva partida?", "AXC", MessageBoxButtons.YesNo) = DialogResult.No Then
                Return
            End If



            AgregaPartida()
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, "AXC", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub grdDetalle_SelectionChanged(sender As System.Object, e As System.EventArgs) Handles grdDetalle.Click
        Try
            If IsNothing(grdDetalle.DataSource) Then
                Return
            End If
            'Dim sRow As Integer = grdDetalle.SelectCell("", "").se.SE SelectedCells(0).RowIndex

            'Dim selectedRow As DataGridViewRow = grdDetalle.Rows(sRow)
            Dim view As ColumnView = CType(grdDetalle.MainView, ColumnView)
            Dim selectedRowHandles As Integer() = view.GetSelectedRows()

            txtPartida.Text = view.GetRowCellDisplayText(selectedRowHandles(0), view.Columns("Partida")) ' selectedRow.Cells("Partida").Value.ToString
            txtPartidaOC.Text = txtOrdenCompra.Text 'selectedRow.Cells("OrdenCompra").Value.ToString
            txtNumParte.Text = view.GetRowCellDisplayText(selectedRowHandles(0), view.Columns("Articulo")) 'selectedRow.Cells("NumParte").Value.ToString
            txtEditaCantidad.Text = view.GetRowCellDisplayText(selectedRowHandles(0), view.Columns("CantidadOrdenada")) 'selectedRow.Cells("CantidadOrdenada").Value.ToString


        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, "AXC", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnEditar_Click(sender As System.Object, e As System.EventArgs) Handles btnEditar.Click
        Try
            If btnEditar.Text = "Editar" Then
                btnEditar.Text = "Cancelar"
                txtEditaCantidad.Enabled = True
                TxtEditaCaracteristica.Enabled = True
                txtEditaCantidad.Select()
                btnGuardar.Enabled = True
                btnEliminar.Enabled = False
            Else
                btnEditar.Text = "Editar"
                txtEditaCantidad.Enabled = False
                TxtEditaCaracteristica.Enabled = False
                btnGuardar.Enabled = False
                btnEliminar.Enabled = True
            End If

            If txtPartida.Text = "" Then
                XtraMessageBox.Show("No hay partida seleccionada", "AXC")
                Return
            End If


        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, "AXC", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnGuardar_Click(sender As System.Object, e As System.EventArgs) Handles btnGuardar.Click
        Try

            If txtEditaCantidad.Text = "" Then
                XtraMessageBox.Show("Debes capturar una cantidad válida", "AXC")
                Return
            End If

            If Double.Parse(txtEditaCantidad.Text) <= 0 Then
                XtraMessageBox.Show("Debes capturar una cantidad válida", "AXC")
                Return
            End If


            If XtraMessageBox.Show("¿Modificar partida?", "AXC", MessageBoxButtons.YesNo) = DialogResult.No Then
                Return
            End If

            ModificarPartida()

        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, "AXC", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnEliminar_Click(sender As System.Object, e As System.EventArgs) Handles btnEliminar.Click
        Try
            If txtPartida.Text = "" Then
                XtraMessageBox.Show("No hay partida seleccionada", "AXC")
                Return
            End If


            If XtraMessageBox.Show("¿Eliminar partida seleccionada?", "AXC", MessageBoxButtons.YesNo) = DialogResult.No Then
                Return
            End If
            EliminarPartida()
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, "AXC", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
#End Region

#Region "METODOS"

    Private Sub ListaClientes(prmBusqueda As String)
        Try
            Dim pResultado As New CResultado
            Dim Cls As New clsCliente

            Cursor.Current = Cursors.WaitCursor
            pResultado = Cls.ResultadoListaClientes(prmBusqueda, My.Settings.Estacion, DatosTemporales.Usuario)
            Cursor.Current = Cursors.Default

            If Not pResultado.Estado Then
                XtraMessageBox.Show(pResultado.Texto)
                Return
            End If
            Dim dt As New DataTable
            Dim ds As New DataSet

            dt = pResultado.Tabla
            ds.Tables.Add(dt)


            cmbCliente.Properties.DataSource = dt
            cmbCliente.Properties.DisplayMember = "Cliente"
            cmbCliente.Properties.ValueMember = "IdCliente"
            cmbCliente.Properties.BestFitMode = BestFitMode.BestFitResizePopup

        Catch ex As Exception
            XtraMessageBox.Show(ex.Message.ToString, "Error", MessageBoxButtons.OK)
        End Try
    End Sub
    Private Sub NuevaOC()
        Try

            Dim pResultado As New CResultado
            Dim pOC As New COrdenCompraR
            Dim ds As DataSet = New DataSet

            Cursor.Current = Cursors.WaitCursor
            pResultado = pOC.NuevaOrdenCompra()
            Cursor.Current = Cursors.Default

            If Not pResultado.Estado Then
                XtraMessageBox.Show(pResultado.Texto, "AXC")
                'grdOP.DataSource = Nothing
                Return
            End If

            txtOrdenCompra.Text = pResultado.Texto

            ' ConsultaOrdenCompra()

        Catch ex As Exception
            Cursor.Current = Cursors.Default
            XtraMessageBox.Show(ex.Message, "AXC", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub ConsultaOrdenCompra()
        Try

            grdDetalle.DataSource = Nothing

            If txtOrdenCompra.Text = "" Then
                XtraMessageBox.Show("No hay Orden de compra seleccionada", "AXC")
                Return
            End If

            Dim pResultado As New CResultado
            Dim pScrap As New COrdenCompraR

            Cursor.Current = Cursors.WaitCursor
            pResultado = pScrap.BuscarOrdenCompra(txtOrdenCompra.Text)
            Cursor.Current = Cursors.Default

            If Not pResultado.Estado Then
                XtraMessageBox.Show(pResultado.Texto, "AXC")
                grdDetalle.DataSource = Nothing
                Return
            End If

            Dim dt As DataTable = New DataTable

            dt = pResultado.Tabla

            cmbCliente.EditValue = dt.Rows(0).Item("IdCliente").ToString

            dt.Columns.Remove("OrdenCompra")
            dt.Columns.Remove("IdCliente")
            'dt.Columns.Remove("FechaProgramada")
            'dt.Columns.Remove("TurnoRecibo")
            'dt.Columns.Remove("FechaRetorno")
            'dt.Columns.Remove("TurnoRetorno")

            grdDetalle.DataSource = dt

            'grdOP.Columns.Remove("CantidadTer")

        Catch ex As Exception
            Cursor.Current = Cursors.Default
            XtraMessageBox.Show(ex.Message, "AXC", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LimpiarCampos()
        txtOrdenCompra.Text = ""

        cmbCliente.EditValue = 0
        cmbNumParte.Properties.DataSource = Nothing
        txtDescripcion.Text = ""

        txtCantidad.Text = ""
        grdDetalle.DataSource = Nothing
        GridView1.Columns.Clear()
        txtPartidaOC.Text = ""
        txtPartida.Text = ""
        txtNumParte.Text = ""
        txtEditaCantidad.Text = ""

        btnGuardar.Enabled = False
    End Sub

    'Private Sub ListaProveedores()
    '    Try
    '        Dim pResultado As New CResultado
    '        Dim pOC As New COrdenCompraR

    '        Cursor.Current = Cursors.WaitCursor
    '        pResultado = pOC.ListaProveedores()
    '        Cursor.Current = Cursors.Default

    '        If Not pResultado.Estado Then
    '            XtraMessageBox.Show(pResultado.Texto, "AXC")
    '            Return
    '        End If

    '        Dim dt As DataTable = New DataTable
    '        dt = pResultado.Tabla


    '        cmbProveedor.Properties.ValueMember = "CodigoProveedor"
    '        cmbProveedor.Properties.DisplayMember = "NomProveedor"
    '        cmbProveedor.Properties.DataSource = dt


    '    Catch ex As Exception
    '        XtraMessageBox.Show(ex.Message, "AXC", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try

    'End Sub

    'Private Sub ConsultaProveedor()
    '    Try
    '        Dim pResultado As New CResultado
    '        Dim pOC As New COrdenCompraR

    '        Cursor.Current = Cursors.WaitCursor
    '        pResultado = pOC.ConsultaProveedor(cmbProveedor.GetColumnValue("CodigoProveedor").ToString)
    '        Cursor.Current = Cursors.Default

    '        If Not pResultado.Estado Then
    '            XtraMessageBox.Show(pResultado.Texto, "AXC")

    '            Return
    '        End If

    '        Dim dt As DataTable = New DataTable
    '        dt = pResultado.Tabla

    '        txtDireccionCliente.Text = dt.Rows(0).Item("DirProveedor").ToString

    '    Catch ex As Exception
    '        XtraMessageBox.Show(ex.Message, "AXC", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    Private Sub ListaNumParte(ByVal prmCliente As String)
        Try
            Dim pResultado As New CResultado
            Dim pOC As New clsNumParte

            Cursor.Current = Cursors.WaitCursor
            pResultado = pOC.ListaNumPartePorCliente(prmCliente, "@", My.Settings.Estacion, DatosTemporales.Usuario)
            Cursor.Current = Cursors.Default

            If Not pResultado.Estado Then
                XtraMessageBox.Show(pResultado.Texto, "AXC")
                Return
            End If

            Dim dt As DataTable = New DataTable
            dt = pResultado.Tabla

            cmbNumParte.Properties.ValueMember = "IdNumParte"
            cmbNumParte.Properties.DisplayMember = "Artículo"
            cmbNumParte.Properties.DataSource = dt
            cmbNumParte.Properties.BestFitMode = BestFitMode.BestFitResizePopup
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, "AXC", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub BuscaNumParte(prmTexto As String)
        Try
            If prmTexto = "" Then
                prmTexto = "@"
            End If

            Dim pResultado As New CResultado
            Dim pOC As New COrdenCompraR

            Cursor.Current = Cursors.WaitCursor
            pResultado = pOC.FiltroNumParte(prmTexto)
            Cursor.Current = Cursors.Default

            If Not pResultado.Estado Then
                XtraMessageBox.Show(pResultado.Texto, "AXC")
                Return
            End If

            Dim dt As DataTable = New DataTable
            dt = pResultado.Tabla

            cmbNumParte.Properties.ValueMember = "IdNumParte"
            cmbNumParte.Properties.DisplayMember = "NumParte"
            cmbNumParte.Properties.DataSource = dt
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, "AXC", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub ConsultaNumParte()
        Try
            Dim pResultado As New CResultado
            Dim pOC As New COrdenCompraR

            Cursor.Current = Cursors.WaitCursor
            pResultado = pOC.ConsultaNumParte(cmbNumParte.Text)
            Cursor.Current = Cursors.Default

            If Not pResultado.Estado Then
                XtraMessageBox.Show(pResultado.Texto, "AXC")

                Return
            End If

            Dim dt As DataTable = New DataTable
            dt = pResultado.Tabla

            txtDescripcion.Text = dt.Rows(0).Item("DNumParte1").ToString

        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, "AXC", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub AgregaPartida()
        Try

            Dim pResultado As New CResultado
            Dim pOC As New COrdenCompraR
            Dim ds As DataSet = New DataSet



            Cursor.Current = Cursors.WaitCursor
            pResultado = pOC.AgregarPartida(txtOrdenCompra.Text, cmbCliente.Text, cmbNumParte.Text,
                                            txtCantidad.Text,
                                            My.Settings.Estacion, DatosTemporales.Usuario)
            'cmbTipoRec.Text, dRec.ToString("yyyyMMdd"),
            'cmbTurnoRecepcion.Text, dRet.ToString("yyyyMMdd"), cmbTurnoRetorno.Text, TxtCaracteristica.Text,
            Cursor.Current = Cursors.Default

            If Not pResultado.Estado Then
                XtraMessageBox.Show(pResultado.Texto, "AXC")
                'grdOP.DataSource = Nothing
                Return
            End If

            XtraMessageBox.Show("Partida Registrada", "AXC")
            TxtCaracteristica.Text = ""
            txtCantidad.Text = ""
            ConsultaOrdenCompra()

        Catch ex As Exception
            Cursor.Current = Cursors.Default
            XtraMessageBox.Show(ex.Message, "AXC", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ModificarPartida()
        Try

            Dim pResultado As New CResultado
            Dim pOC As New COrdenCompraR
            Dim ds As DataSet = New DataSet

            Cursor.Current = Cursors.WaitCursor
            pResultado = pOC.ModificarPartida(txtOrdenCompra.Text, txtPartida.Text, txtEditaCantidad.Text, My.Settings.Estacion, DatosTemporales.Usuario) 'TxtEditaCaracteristica.Text,
            Cursor.Current = Cursors.Default

            If Not pResultado.Estado Then
                XtraMessageBox.Show(pResultado.Texto, "AXC")
                Return
            End If

            XtraMessageBox.Show("Partida modificada", "AXC")

            ConsultaOrdenCompra()

            txtEditaCantidad.Enabled = False
            txtEditaCantidad.Text = ""
            TxtEditaCaracteristica.Enabled = False
            TxtEditaCaracteristica.Text = ""
            btnGuardar.Enabled = False
            btnEditar.Enabled = True
            btnEditar.Text = "Editar"
            btnEliminar.Enabled = True
        Catch ex As Exception
            Cursor.Current = Cursors.Default
            XtraMessageBox.Show(ex.Message, "AXC", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub EliminarPartida()
        Try

            Dim pResultado As New CResultado
            Dim pOC As New COrdenCompraR
            Dim ds As DataSet = New DataSet

            Cursor.Current = Cursors.WaitCursor
            pResultado = pOC.EliminarPartida(txtOrdenCompra.Text, txtPartida.Text, My.Settings.Estacion, DatosTemporales.Usuario)
            Cursor.Current = Cursors.Default

            If Not pResultado.Estado Then
                XtraMessageBox.Show(pResultado.Texto, "AXC")
                Return
            End If

            XtraMessageBox.Show("Partida Eliminada", "AXC")

            If pResultado.Texto = "1" Then
                ConsultaOrdenCompra()
            Else
                XtraMessageBox.Show("Orden de compra eliminada", "AXC", MessageBoxButtons.OK)
                LimpiarCampos()
                txtOrdenCompra.Select()

            End If



        Catch ex As Exception
            Cursor.Current = Cursors.Default
            XtraMessageBox.Show(ex.Message, "AXC", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub llenaVariables(prmOrden As String)
        Try
            sOrden = prmOrden
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message, "AXC", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    'Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
    '    Try
    '        FrmVistaRecepciones.ShowDialog()
    '        If sOrden <> "" Then
    '            txtOrdenCompra.Text = sOrden
    '            ConsultaOrdenCompra()
    '        End If
    '    Catch ex As Exception
    '       XtraMessageBox.Show(ex.Message, "AXC", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    End Try
    'End Sub





    Private Sub cmbCliente_EditValueChanged(sender As Object, e As EventArgs) Handles cmbCliente.EditValueChanged
        Try
            Dim text As String = cmbCliente.Text
            If text.Equals("System.Data.DataRowView") Then
                Return
            End If
            ListaNumParte(cmbCliente.Text)
        Catch ex As Exception
            XtraMessageBox.Show(ex.Message.ToString, "AXC", MessageBoxButtons.OK)
        End Try
    End Sub

    Private Sub FrmCreaRecepcion_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Me.Dispose(True)
    End Sub





#End Region




End Class