import React, { Suspense } from 'react';
import { Container } from 'react-bootstrap';
import AppRoutes from 'routes';
import QueryCachProvider from 'providers/QueryCachProvider';
import Header from 'components/Header';
import Footer from 'components/Footer';
import Loading from 'components/Loading';

export default function Main() {
    return (
        <QueryCachProvider>
            <main className="main-layout">
                <Header />
                <Container className="page-wrapper" fluid="xl">
                    <Suspense fallback={<Loading className="page-loading" />}>
                        <AppRoutes />
                    </Suspense>
                </Container>
                <Footer />
            </main>
        </QueryCachProvider>
    );
}
