import { Component, OnInit } from '@angular/core';
import { AbstractControl, AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { ValidatorField } from '@app/helpers/ValidatorField';
import { User } from '@app/models/identity/User';
import { AccountService } from '@app/services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {


  user = {} as User;

  form: FormGroup = new FormGroup({}) ;

  get f(): any {
    return this.form.controls;
  }

  constructor(private fb:FormBuilder,
              private toaster:ToastrService,
              private accountService:AccountService,
              private router:Router) { }

  ngOnInit(): void {
    this.validation();
  }

  public validation(): void{

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmPassword')
    };

    this.form = this.fb.group({
      primeiroNome:   ['', Validators.required],
      ultimoNome:     ['', Validators.required],
      email:          ['',[Validators.required, Validators.email]],
      usuario:     ['', Validators.required],
      password:          ['',
        [Validators.required, Validators.minLength(4)]
      ],
      confirmPassword:   ['', Validators.required]

    }, formOptions)
  }

  register(): void {
    this.user = { ...this.form.value};
    this.accountService.register(this.user).subscribe(
      () => this.router.navigateByUrl('/dashboard'),
      (error: any) => this.toaster.error(error.error)

    )
  }

}
