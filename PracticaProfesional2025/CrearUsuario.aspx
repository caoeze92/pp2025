<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CrearUsuario.aspx.cs" Inherits="PracticaProfesional2025.CrearUsuario" %>
<!doctype html>
<html lang="en">
  <head>
  	<title>NUEVO USUARIO - Sistema de control de Inventario Institucional - ISFDyT 46</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">

	<link href="https://fonts.googleapis.com/css?family=Lato:300,400,700&display=swap" rel="stylesheet">

	<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css">
	
	<link rel="stylesheet" href="Contenido/registro/css/Registro.css">

	</head>
	<body>
	<section class="ftco-section">
		<div class="container">
			<div class="row justify-content-center">
				<div class="col-md-6 text-center mb-5">
					<h2 class="heading-section">REGISTRO</h2>
				</div>
			</div>
			<div class="row justify-content-center">
				<div class="col-md-7 col-lg-5">
					<div class="wrap">
						<div class="img" style="background-image: url(Contenido/registro/images/bg-1.jpg);"></div>
						<div class="login-wrap p-4 p-md-5">
                          <div class="form-group d-md-flex">
		                <p class="text-center">Seccion de registro de Nuevas Cuentas.</p>
		                </div>
			      	<div class="d-flex">
			      		<div class="w-100">
			      			<h6 class="mb-4">Ingresa tus datos:</h6>
			      		</div>
								<div class="w-100">
									<p class="social-media d-flex justify-content-end">
										<a href="https://www.instagram.com/instituto.46" class="social-icon d-flex align-items-center justify-content-center"><span class="fa fa-instagram"></span></a>
										<a href="http://www.instituto46.edu.ar/" class="social-icon d-flex align-items-center justify-content-center"><span class="fa fa-globe"></span></a>
                                        <a href="mailto:info@instituto46.edu.ar" class="social-icon d-flex align-items-center justify-content-center"><span class="fa fa-envelope"></span></a>
									</p>
								</div>
			      	</div>
                   <form id="Form1" action="#" class="signin-form" runat="server">
			      		<div class="form-group mt-4">
			      			<!--<input type="text" class="form-control" required>-->
                            <asp:TextBox ID="nTxtNombre" runat="server" class="form-control" required></asp:TextBox>
			      			<label class="form-control-placeholder" for="nTxtNombre">Nombre:</label>
			      		</div>
                        <div class="form-group mt-4">
			      			<!--<input type="text" class="form-control" required>-->
                            <asp:TextBox ID="nTxtApellido" runat="server" class="form-control" required></asp:TextBox>
			      			<label class="form-control-placeholder" for="nTxtApellido">Apellido:</label>
			      		</div>
                        <div class="form-group mt-4">
			      			<!--<input type="text" class="form-control" required>-->
                            <asp:TextBox ID="nTxtEmail" runat="server" class="form-control" required></asp:TextBox>
			      			<label class="form-control-placeholder" for="nTxtEmail">Email:</label>
			      		</div>
                        <div class="form-group mt-4">
		              <!--<input id="password-field" type="password" class="form-control" required>-->
                      <asp:TextBox ID="nTxtPassword" textMode= "Password" runat="server" class="form-control" required></asp:TextBox>
		              <label class="form-control-placeholder" for="nTxtPassword">Constraseña:</label>
		              <span toggle="#password-field" class="fa fa-fw fa-eye field-icon toggle-password"></span>
		            </div>
                    <div class="form-group mt-4">
			      			<!--<input type="text" class="form-control" required>-->
                            <asp:TextBox ID="nTxtTelefono" runat="server" class="form-control" required></asp:TextBox>
			      			<label class="form-control-placeholder" for="nTxtTelefono">Telefono:</label>
			      		</div>
		            <div class="form-group">
		            	<!-- <button type="submit" class="form-control btn btn-primary rounded submit px-3">Sign In</button>-->
                        <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" class="form-control btn btn-primary rounded submit px-" Text="Registrarse"></asp:Button>
		            </div>
		          </form>
		          <p class="text-center">¿Ya estás registrado?<a target="_self" href="login.aspx"><br />Ingresar</a></p>
		        </div>
		      </div>
				</div>
			</div>
		</div>
	</section>

	<script src="Contenido/registro/js/jquery.min.js"></script>
  <script src="Contenido/registro/js/popper.js"></script>
  <script src="Contenido/registro/js/bootstrap.min.js"></script>
  <script src="Contenido/registro/js/main.js"></script>

	</body>
</html>
