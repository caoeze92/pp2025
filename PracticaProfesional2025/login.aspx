<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="PracticaProfesional2025.login" %>
<!doctype html>
<html lang="en">
  <head>
  	<title>Sistema de control de Inventario Institucional - ISFDyT 46</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">

	<link href="https://fonts.googleapis.com/css?family=Lato:300,400,700&display=swap" rel="stylesheet">

	<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css">
	
	<link rel="stylesheet" href="Contenido/login/css/Login.css">

	</head>
	<body>
	<section class="ftco-section">
		<div class="container">
			<div class="row justify-content-center">
				<div class="col-md-6 text-center mb-5">
					<h2 class="heading-section">Sistema de control <br />Inventario Institucional</h2>
                    <h3 class="heading-section">ISFDyT 46</h3>
                    
				</div>
			</div>
			<div class="row justify-content-center">
				<div class="col-md-7 col-lg-5">
					<div class="wrap">
						<div class="img" style="background-image: url(Contenido/images/dtc.jpeg);"></div>
						<div class="login-wrap p-4 p-md-5">
			      	<div class="d-flex">
			      		<div class="w-100">
			      			<h3 class="mb-4">Cuenta:</h3>
			      		</div>
								<div class="w-100">
									<p class="social-media d-flex justify-content-end">
										<a href="https://www.instagram.com/instituto.46" class="social-icon d-flex align-items-center justify-content-center" target="_blank"><span class="fa fa-instagram"></span></a>
										<a href="http://www.instituto46.edu.ar/" class="social-icon d-flex align-items-center justify-content-center" target="_blank"><span class="fa fa-globe"></span></a>
                                        <a href="mailto:info@instituto46.edu.ar" class="social-icon d-flex align-items-center justify-content-center" target="_blank"><span class="fa fa-envelope"></span></a>
									</p>
								</div>
			      	</div>
							<form action="#" class="signin-form" runat="server">
			      		<div class="form-group mt-4">
			      			<!--<input type="text" class="form-control" required>-->

                            <asp:TextBox ID="logTxtEmail" runat="server" class="form-control" required></asp:TextBox>
			      			<label class="form-control-placeholder" for="nTxtEmail">Email:</label>

			      		</div>
		            <div class="form-group mt-4">
		              <!--<input id="password-field" type="password" class="form-control" required>-->

                      <asp:TextBox ID="logTxtPassword" textMode= "Password" runat="server" class="form-control" required></asp:TextBox>
		              <label class="form-control-placeholder" for="logTxtPassword">Password</label>

		              <span toggle="#password-field" class="fa fa-fw fa-eye field-icon toggle-password"></span>
		            </div>
		            <div class="form-group">
		            	<!-- <button type="submit" class="form-control btn btn-primary rounded submit px-3">Sign In</button>-->
                        <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" class="form-control btn btn-primary rounded submit px-" Text="Ingresar"></asp:Button>
		            </div>
		            <div class="form-group d-md-flex">
		            	<div class="w-50 text-left">
			            	<label class="checkbox-wrap checkbox-primary mb-0">Recordar Contraseña
									  <input type="checkbox" checked>
									  <span class="checkmark"></span>
										</label>
									</div>
									<div class="w-50 text-md-right">
										<a href="#">Olvide mi contraseña</a>
									</div>
		            </div>
		          </form>
		          <p class="text-center">¿Aun no estás registrado? <a target="_self" href="CrearUsuario.aspx"><br />Nuevo usuario</a></p>
		        </div>
		      </div>
				</div>
			</div>
		</div>
	</section>

	<script src="Contenido/login/js/jquery.min.js"></script>
  <script src="Contenido/login/js/popper.js"></script>
  <script src="Contenido/login/js/bootstrap.min.js"></script>
  <script src="Contenido/login/js/main.js"></script>

	</body>
</html>

