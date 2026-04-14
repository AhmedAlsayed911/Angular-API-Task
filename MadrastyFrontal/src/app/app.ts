import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Header } from '../Components/header/header';
import { Footer } from '../Components/footer/footer';

@Component({
  selector: 'app-root',
  imports: [Header, Footer, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {}
