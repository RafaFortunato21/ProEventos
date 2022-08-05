import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';


import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';


import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  evento = {} as Evento;
  estadoSalvar = 'post'
  form: FormGroup = new FormGroup({}) ;

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
              private router: ActivatedRoute,
              private eventoService:EventoService,
              private spinner:NgxSpinnerService,
              private toast:ToastrService) {
    this.localeService.use('pt-br');
  }


  public carregarEvento(): void {
    const eventoIdParam = this.router.snapshot.paramMap.get('id');

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

      if (this.estadoSalvar === 'post' || this.estadoSalvar === 'put' ) {
        this.eventoService[this.estadoSalvar](this.evento).subscribe(
          (result) => this.toast.success('Evento salvo com sucesso.', 'Sucesso!'),
          (error) => {
            console.error(error);
            this.spinner.hide();
            this.toast.error('Falha ao cadastrar o Evento', 'Erro!');
          },
          () => this.spinner.hide()
        );
      }



    }
  }

  ngOnInit(): void {
    this.carregarEvento();
    this.validation();
    console.log(this.evento.dataEvento);
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

  public cssValidator(form: FormControl): any {
    return {'is-invalid': form.errors && form?.touched }
  }

}
