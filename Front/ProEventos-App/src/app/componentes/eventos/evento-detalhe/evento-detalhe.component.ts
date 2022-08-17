 import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { FormattedError } from '@angular/compiler';

import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lote';
import { EventoService } from '@app/services/evento.service';
import { LoteService } from '@app/services/lote.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  eventoId: number;
  evento = {} as Evento;
  estadoSalvar = 'post'
  form: FormGroup = new FormGroup({}) ;
  modalRef: BsModalRef;
  loteAtual = {id: 0, nome: '', indice: 0};
  imageURl = 'assets/img/upload.png';
  file: File;


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
              private loteService:LoteService,
              private spinner:NgxSpinnerService,
              private toast:ToastrService,
              private router: Router,
              private modalService: BsModalService) {
    this.localeService.use('pt-br');
  }


  public carregarEvento(): void {
    this.eventoId = +this.activatedRouter.snapshot.paramMap.get('id');

    if (this.eventoId != null && this.eventoId != 0 ) {
      this.spinner.show();

        this.estadoSalvar = 'put';

      this.eventoService.getEventoById(this.eventoId).subscribe(
        (evento: Evento) => {
          this.evento = {...evento};
          this.form.patchValue(this.evento);
          if (this.evento.imageURL !== '') {
              this.imageURl = environment.apiURL + 'resources/images/' + this.evento.imageURL
          }
          this.carregarLotes();
        },
        (error:any) => {
          console.error(error);
          this.toast.error('Erro ao tentar carregar Evento');
        }
      ).add(() => this.spinner.hide() );
    }
  }

  public carregarLotes(): void {
    this.loteService.getLotesByEventoId(this.eventoId).subscribe(
      (lotesRetorno: Lote[]) => {
        lotesRetorno.forEach(lote => {
          this.lotes.push(this.criarLote(lote));
        })
      },
      (error: any) => {
        this.toast.error('Erro ao Carregar os lotes', 'Erro!');
      }

    ).add(() => this.spinner.hide() )

  }

  public salvarEvento(): void {

    if (this.form.valid) {
      this.spinner.show();

      this.evento = this.estadoSalvar === 'post'
                    ? {...this.form.value}
                    : {id: this.evento.id,...this.form.value};

      this.eventoService[this.estadoSalvar](this.evento).subscribe(
        (eventoRetorno: Evento) => {
          this.toast.success('Evento salvo com sucesso.', 'Sucesso!');
          this.router.navigate([`eventos/detalhe/${eventoRetorno.id}`])
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

  public salvarLotes(): void {
    this.spinner.show();
    if (this.form.controls.lotes.valid) {
      this.loteService.saveLote(this.eventoId, this.form.value.lotes)
        .subscribe(
        () => {
          this.toast.success('Lotes Salvos com Sucesso!', 'Sucesso');
          //this.lotes.reset();
        },
        (error: any) => {
          this.toast.error('Erro ao tentar salvar lotes', 'Error');
          console.error(error);
        }
        ).add(() => this.spinner.hide() )




      this.spinner.hide();
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
      imageURL:   [''],
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

  public retornaTituloLote(nome: string): string {
    return nome === null ||  nome === ''
      ? '[Nome do lote]'
      : nome;
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(form: FormControl | AbstractControl): any {
    return {'is-invalid': form?.errors && form?.touched }
  }


  public removerLote(template:TemplateRef<any>,
                      indice:number): void {

    this.loteAtual.id = this.lotes.get(indice + '.id').value;
    this.loteAtual.nome = this.lotes.get(indice + '.nome').value;
    this.loteAtual.indice = indice;

     this.modalRef = this.modalService.show(template, {class:'modal-sm' })


     ;



    }

    public confirmDeleteLote(): void {
      this.modalRef.hide();
      this.spinner.show();

      this.loteService.deleteLote(this.eventoId, this.loteAtual.id)
      .subscribe(
        () => {
          this.toast.success('Excluido com sucesso.')
          this.lotes.removeAt(this.loteAtual.indice);
        },
        (error:any) => {
          this.toast.error(`Erro ao tentar deletar o Lote ${this.loteAtual.id} `);
          console.error(error);
        }
      ).add(() => this.spinner.hide() )


    this.modalRef.hide();
  }

  public declineDeleteLote(): void {
    this.modalRef.hide();
  }


  onFileChange(ev: any): void {
    const reader = new FileReader();

    reader.onload = (event: any) => this.imageURl = event.target.result;

    this.file = ev.target.files;

    reader.readAsDataURL(this.file[0]);

    this.uploadImage();

  }


  uploadImage(): void {
    this.spinner.show();
    this.eventoService.postUpload(this.eventoId, this.file).subscribe(
      () => {
        this.carregarEvento();
        this.toast.success('Imagem atualizada com sucesso','Sucesso');
      },
      (error:any) => {
        this.toast.error('Erro ao fazer upload de imagem','Erro!');
        console.error(error);
      }
    ).add(() => this.spinner.hide() )

  }



}
