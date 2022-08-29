import { ThrowStmt } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {

  userUpdate = {} as UserUpdate;
  form: FormGroup = new FormGroup({}) ;


  get f(): any {
    return this.form.controls;
  }

  constructor(private fb:FormBuilder,
              public accountService:AccountService,
              private router:Router,
              private toaster:ToastrService,
              private spinner:NgxSpinnerService) { }

  ngOnInit() {
    this.validation();
    this.carregarUsuario();
  }


  private carregarUsuario(): void{
    this.spinner.show();

    this.accountService.getUser().subscribe(
      (userRetorno:UserUpdate) => {
        console.log(userRetorno);
        this.userUpdate = userRetorno;
        this.form.patchValue(this.userUpdate);
        //this.toaster.success('Usuario Carregado com sucesso', 'Sucesso');
      },
      (error:any) => {
        console.error(error);
        this.toaster.error('Usuario não carregado','Erro');
        this.router.navigate(['/dashboard']);
      }
    )
    .add(() => this.spinner.hide());


  }
  public validation(): void{

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmPassword')
    };

    this.form = this.fb.group({
      userName:   [''],
      titulo:            ['NaoInformado', Validators.required],
      primeiroNome:      ['', Validators.required],
      ultimoNome:        ['', Validators.required],
      email:             ['',[Validators.required, Validators.email]],
      phoneNumber:          ['', Validators.required],
      funcao:            ['NaoInformado', Validators.required],
      descricao:         ['', Validators.required],
      password:          ['',[Validators.required, Validators.minLength(4)]],
      confirmPassword:   ['', Validators.required]

    }, formOptions)
  }

  onSubmit(): void{
    this.atualizarUsuario();

  }

  public atualizarUsuario(){
    this.userUpdate = { ...this.form.value}
    this.spinner.show();

    this.accountService.updateUser(this.userUpdate).subscribe(
      () => this.toaster.success('Usuario atualizado', 'Sucesso'),
      (error:any) => {
        this.toaster.error(error.error, 'Erro');
        console.error(error);
      }
    ).add(() => this.spinner.hide());
  }



  public resetForm(event: any) : void {
    event.preventDefault();
    this.form.reset();
  }

}
