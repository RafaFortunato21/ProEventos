import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {


  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];
  public largunraImagem = 150;
  public margemImagem = 2;
  public mostrarImagem = true;
  modalRef?: BsModalRef;
  public eventoId = 0;



  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
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
    this.carregarEventos();
    this.spinner.show();


  }

  public carregarEventos(): void {
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



  openModal(event: any, template: TemplateRef<any>, eventoId:number) : void {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  showSuccess() {
    this.toastr.success('Evento deletado com sucesso!', 'Deletado!');
  }

  confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();

    this.eventoService.deleteEvento(this.eventoId).subscribe(
      (result: any) => {
        if (result.message === 'Deletado') {
          this.showSuccess();
          this.carregarEventos();
        }
      },
      (error:any) => {
        console.error(error);
        this.toastr.error(`Falha ao excluir o Evento ${this.eventoId} `, 'Erro!');
        this.spinner.hide();
      },

    ).add(() => this.spinner.hide(),);

  }

  decline(): void {
    this.modalRef?.hide();
  }

  detalheEvento(id: number): void {
    this.router.navigate([`eventos/detalhe/${id}`]);
  }


}
