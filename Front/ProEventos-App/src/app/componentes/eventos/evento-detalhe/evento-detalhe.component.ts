 import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';


import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { Lote } from '@app/models/Lote';


import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { FormattedError } from '@angular/compiler';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  evento = {} as Evento;
  estadoSalvar = 'post'
  form: FormGroup = new FormGroup({}) ;


  get modoEditar(): boolean {
    return this.estadoSalvar == 'put';
  }


  get lotes(): FormArray{
    return this.form.get('lotes') as FormArray;
  }


  get f(): any {
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY HH:mm:ss',
      isAnimated: true,
      containerClass: 'theme-default',
      showWeekNumbers: false

    }
  }

  constructor(private fb:FormBuilder, private localeService: BsLocaleService,
              private activatedRouter: ActivatedRoute,
              private eventoService:EventoService,
              private spinner:NgxSpinnerService,
              private toast:ToastrService,
              private router: Router) {
    this.localeService.use('pt-br');
  }


  public carregarEvento(): void {
    const eventoIdParam = this.activatedRouter.snapshot.paramMap.get('id');

    if (eventoIdParam != null ) {
      this.spinner.show();

        this.estadoSalvar = 'put';

      this.eventoService.getEventoById(+eventoIdParam).subscribe(
        (evento: Evento) => {
          this.evento = {...evento};
          this.form.patchValue(this.evento);
        },
        (error:any) => {
          console.error(error);
          this.spinner.hide();
          this.toast.error('Erro ao tentar carregar Evento');
        },
        () => this.spinner.hide(),
      );
    }
  }

  public salvarAlteracao(): void {
    this.spinner.show();

    if (this.form.valid) {

      this.evento = this.estadoSalvar === 'post'
                    ? {...this.form.value}
                    : {id: this.evento.id,...this.form.value};

      this.eventoService[this.estadoSalvar](this.evento).subscribe(
        () => {
          this.toast.success('Evento salvo com sucesso.', 'Sucesso!');
        },
        (error: any) => {
          console.error(error);
          this.spinner.hide();
          this.toast.error('Falha ao cadastrar o Evento', 'Erro!');
        },
        () => this.spinner.hide()
      );


    }
  }

  ngOnInit(): void {
    this.carregarEvento();
    this.validation();
  }


  public validation(): void{
    this.form = this.fb.group({
      tema:       ['',[Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local:      ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['',[Validators.required, Validators.max(120000)]],
      imageURL:   ['', Validators.required],
      telefone:   ['', Validators.required],
      email:      ['',[Validators.required, Validators.email]],
      lotes: this.fb.array([])


    });
  }

  adicionarLote(): void {
    this.lotes.push(this.criarLote({id: 0} as Lote));
  }

  criarLote(lote: Lote): FormGroup {
    return this.fb.group({
      id:[lote.id],
      nome:[lote.nome, Validators.required],
      preco:[lote.preco, Validators.required],
      quantidade:[lote.quantidade, Validators.required],
      dataInicio:[lote.dataInicio, Validators.required],
      dataFim:[lote.dataFim, Validators.required]
    })
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(form: FormControl | AbstractControl): any {
    return {'is-invalid': form?.errors && form?.touched }
  }

}
