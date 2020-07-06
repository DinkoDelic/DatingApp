import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guards/auth.guard';

// angular router operates on a first match wins system. Order of paths important( we can't have wildcard path on top)
export const appRoutes: Routes = [
    {path: '', component: HomeComponent},
    {
        // Using a 'dummy path' (no values) to match with our 3 children path and protected them with AuthGuard
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            {path: 'members', component: MemberListComponent},
            {path: 'messages', component: MessagesComponent},
            {path: 'lists', component: ListsComponent},
        ]
    },
    // wildcard path that redirects to home when the url path doesn't match any of the previous options
    {path: '**', redirectTo: '', pathMatch: 'full'}
];