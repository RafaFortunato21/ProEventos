import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { environment } from '@environments/environment';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {


  public eventos: Evento[] = [];
  public largunraImagem = 150;
  public margemImagem = 2;
  public mostrarImagem = true;
  modalRef?: BsModalRef;
  public eventoId = 0;
  public pagination = {} as Pagination



  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
    ) { }

    termoBucaChanged: Subject<string> = new Subject<string>();

    public filtrarEventos(event: any): void {

      if (this.termoBucaChanged.observers.length == 0) {
        this.termoBucaChanged.pipe(debounceTime(1000)).subscribe(
          filrarPor => {
            this.spinner.show();
            this.eventoService
            .getEvento(
              this.pagination.currentPage,
              this.pagination.itemsPerPage,
              filrarPor
              ).subscribe(
                (response: PaginatedResult<Evento[]>) =>  {
                  this.eventos = response.result;
                },
                (error: any) => {
                  this.spinner.hide();
                  this.toastr.error('Erro ao carregar os eventos!', 'Erro!')
                }
                ).add(() => this.spinner.hide());
              }
          )
      }
      this.termoBucaChanged.next(event.value);

    }


          public ngOnInit(): void {
            this.pagination = {currentPage: 1, itemsPerPage:3, totalItems:10} as Pagination;
            this.spinner.show();
            this.carregarEventos();


          }

          public carregarEventos(): void {
            this.eventoService.getEvento(this.pagination.currentPage,
              this.pagination.itemsPerPage).subscribe(
                (response: PaginatedResult<Evento[]>) =>  {
                  this.eventos = response.result;
                },
                (error: any) => {
                  this.spinner.hide();
                  this.toastr.error('Erro ao carregar os eventos!', 'Erro!')
                }).add(() => this.spinner.hide());
              }

              public exibirImagem() : void {
                this.mostrarImagem = !this.mostrarImagem;

              }

              public mostrarImagemTela(imagemURL: string): string {
                return (imagemURL !== '')
                ? `${environment.apiURL}resources/images/${imagemURL}`
                : 'assets/img/semImagem.png'


              }



              openModal(event: any, template: TemplateRef<any>, eventoId:number) : void {
                event.stopPropagation();
                this.eventoId = eventoId;
                this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
              }

              showSuccess() {
                this.toastr.success('Evento deletado com sucesso!', 'Deletado!');
              }

              public pageChanged(event):void {
                this.pagination.currentPage = event.page;
                this.carregarEventos();
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
