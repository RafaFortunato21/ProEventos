import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  form: FormGroup = new FormGroup({}) ;

  get f(): any {
    return this.form.controls;
  }

  constructor(private fb:FormBuilder) { }

  ngOnInit(): void {
    this.validation();
  }

  public validation(): void{
    this.form = this.fb.group({
      tema: ['',
        [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],

      local:      ['', Validators.required],

      dataEvento: ['', Validators.required],
      qtdPessoas: ['',[Validators.required, Validators.max(120000)]],

      imageURL:   ['', Validators.required],
      telefone:   ['', Validators.required],
      email:      ['',[Validators.required, Validators.email]
      ]
    })
  }

  public resetForm(): void {
    this.form.reset();
  }

}