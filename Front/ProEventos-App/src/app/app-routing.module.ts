import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContatosComponent } from './componentes/contatos/contatos.component';
import { DashboardComponent } from './componentes/dashboard/dashboard.component';
import { EventoDetalheComponent } from './componentes/eventos/evento-detalhe/evento-detalhe.component';
import { EventoListaComponent } from './componentes/eventos/evento-lista/evento-lista.component';
import { EventosComponent } from './componentes/eventos/eventos.component';
import { PalestrantesComponent } from './componentes/palestrantes/palestrantes.component';
import { PerfilComponent } from './componentes/user/perfil/perfil.component';
import { LoginComponent } from './componentes/user/login/login.component';
import { RegistrationComponent } from './componentes/user/registration/registration.component';
import { UserComponent } from './componentes/user/user.component';
import { AuthGuard } from './guard/auth.guard';
import { HomeComponent } from './componentes/home/home.component';

const routes: Routes = [
  {   path: '', redirectTo: 'home', pathMatch: 'full'     },



  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {   path: 'user', redirectTo: 'user/perfil'},
      {   path: 'user/perfil', component: PerfilComponent               },

      {   path: 'eventos', redirectTo: 'eventos/lista'},
      {   path: 'eventos', component: EventosComponent,
      children: [
        {path: 'detalhe/:id', component: EventoDetalheComponent},
        {path: 'detalhe', component: EventoDetalheComponent},
        {path: 'lista', component: EventoListaComponent}
      ]

      },
      {   path: 'contatos', component: ContatosComponent           },
      {   path: 'dashboard', component: DashboardComponent         },
      {   path: 'palestrantes', component: PalestrantesComponent   },
    ]
  },
  {   path: 'user', component: UserComponent,
      children: [
        { path: 'login', component: LoginComponent},
        { path: 'registration', component: RegistrationComponent}
      ]
  },


  {   path: 'home', component: HomeComponent           },

  {   path: '', redirectTo: 'dashboard', pathMatch: 'full'     },
  {   path: '**', redirectTo: 'dashboard', pathMatch: 'full'     }


];



@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
