import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Evento } from 'src/app/models/Evento';
import { EventoService } from 'src/app/services/evento.service';


@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']

})
export class EventosComponent implements OnInit {

  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];
  public largunraImagem = 150;
  public margemImagem = 2;
  public mostrarImagem = true;
  modalRef?: BsModalRef;



  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) { }

  private _filtroListado = '';

  public get filtroLista(): string {
    return this._filtroListado;
  }

  public set filtroLista(value: string){
    this._filtroListado = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this._filtroListado) : this.eventos
  }

  public filtrarEventos(filtrarPor: string): Evento[] {
      filtrarPor = filtrarPor.toLocaleLowerCase();
      return this.eventos.filter(
        (evento : Evento) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
        evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
      )
  }


  public ngOnInit(): void {
    this.getEventos();
    this.spinner.show();


  }

  public getEventos(): void {
    this.eventoService.getEvento().subscribe({
      next: (_eventos: Evento[]) =>  {
        this.eventos = _eventos;
        this.eventosFiltrados  = this.eventos
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os eventos!', 'Erro!');
      },
      complete: () => this.spinner.hide()
    });
  }

  public exibirImagem() : void {
    this.mostrarImagem = !this.mostrarImagem;

  }


  openModal(template: TemplateRef<any>) : void {
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  showSuccess() {
    this.toastr.success('Evento deletado com sucesso!', 'Deletado!');
  }

  confirm(): void {
    this.modalRef?.hide();
    this.showSuccess();
  }

  decline(): void {
    this.modalRef?.hide();
  }


}
